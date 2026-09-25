using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.Commands;

public static class ImaginationCmd
{
    internal const int DefaultConcurrentFigmentLimit = 2;

    public static IEnumerable<Figment> GetFigments(this Player player)
    {
        return player.Creature.Pets
            .Where(c => c.Monster is Figment { IsAvailable: true })
            .Select(c => (c.Monster as Figment)!)
            .OrderBy(f => f.PreventedFromPopping)
            .ThenBy(f => f.Timestamp);
    }

    public static async Task ImagineRandom<T>(PlayerChoiceContext choiceContext, Player owner) where T : FigmentPower
    {
        var manager = MainFile.CombatManagerFactory.GetOrCreate(owner);
        var figment = manager.FilterFigment(owner, ModelDb.Power<T>());
        if (figment is null)
        {
            MainFile.Logger.Error($"No figment found with starting power {typeof(T)}. Cannot imagine anything");
            return;
        }

        await manager.ImagineFigment(choiceContext, owner, figment);
    }

    public static async Task Imagine<T>(PlayerChoiceContext choiceContext, Player owner) where T : Figment
    {
        var manager = MainFile.CombatManagerFactory.GetOrCreate(owner);
        var figment = ModelDb.Monster<T>();
        await manager.ImagineFigment(choiceContext, owner, figment);
    }

    internal static async Task Imagine(PlayerChoiceContext choiceContext, Player owner, Figment figmentModel,
        FigmentSlotMap slotMap)
    {
        var monster = figmentModel.ToMutable();
        var creature = owner.Creature.CombatState?.CreateCreature(monster, owner.Creature.Side, null);
        if (creature is null || monster is not Figment newFigment)
        {
            MainFile.Logger.Error("Could not create figment for unknown reason.");
            return;
        }

        // Start a timer to put a min bound on the time we spend here (for visual clarity),
        // then init the figment (set HP, add to owner, etc.)
        MainFile.Logger.Info($"Adding {newFigment.GetType().Name} instance to player.");
        var imagineTimerTask = Cmd.CustomScaledWait(0.50f, 0.75f);
        await newFigment.Initialize(owner);

        // Sort figments oldest to newest, including the one just added
        var figmentBuffer = owner.GetFigments().ToArray();
        var slotBuffer = new int[figmentBuffer.Length];

        // Determine if the player will have more live figments than allowed
        //      It may be the case that there were *already* more figments than allowed
        //      Maybe a power allowing it was removed, etc.
        //      We only care about replacing *the* oldest over the limit; the rest can stay until otherwise removed
        var limit = MainFile.CombatManagerFactory.GetOrCreate(owner).ConcurrentFigmentLimit;
        var oldestPopping = figmentBuffer.Length > limit && figmentBuffer[0] != newFigment;
        var liveRange = oldestPopping ? 1.. : ..;
        var oldestFigment = figmentBuffer[0];
        if (oldestPopping) oldestFigment.MarkForPopping();

        // Init and sort spans
        var allFigments = figmentBuffer.AsSpan();
        var allFigmentSlots = slotBuffer.AsSpan();
        var liveFigments = allFigments[liveRange];
        var liveFigmentSlots = allFigmentSlots[liveRange];
        liveFigments.Sort((x, y) =>
        {
            // Sort by hit priority from back (low) to front (high)
            var hpDelta = x.Creature.CurrentHp - y.Creature.CurrentHp;
            if (hpDelta != 0) return hpDelta;
            return (x.Timestamp ?? 0) - (y.Timestamp ?? 0);
        });

        // Assign slots to live figments, avoiding repositions if possible
        // Keep the popping figment at its current slot regardless
        var slotsAvailable = slotMap.Slots.Count;
        var repositionNeeded = !TryFillSlotsWithoutRepositioning(liveFigments, liveFigmentSlots, slotsAvailable);
        if (repositionNeeded) FillCleanSlots(liveFigments, liveFigmentSlots, slotsAvailable);
        if (oldestPopping) allFigmentSlots[0] = oldestFigment.CurrentSlotIndex ?? 0;

        // Physically place *all* figments into assigned slots (even the popping figment, which will then move to its deathbed),
        // creating reposition tweens as needed
        if (owner.Creature.GetCreatureNode() is { } nOwner)
        {
            var parent = nOwner.GetParent<CanvasItem>();
            parent.YSortEnabled = true;
            nOwner.YSortEnabled = true;

            for (var i = 0; i < allFigments.Length; i += 1)
                allFigments[i].SetSlot(allFigmentSlots[i], slotMap, nOwner);
        }

        // Convert popping figment HP into block
        if (oldestPopping)
        {
            var block = new BlockVar(oldestFigment.Creature.CurrentHp, ValueProp.Unpowered);
            await CreatureCmd.GainBlock(owner.Creature, block, null);
            await oldestFigment.Pop();
        }

        // Finish new figment init
        await newFigment.SetMoveAndPowers(choiceContext);

        // Heal new figment by vigor
        var ownerVigor = owner.Creature.GetPower<VigorPower>();
        var hpToHeal = Math.Min(
            newFigment.Creature.MaxHp - newFigment.Creature.CurrentHp,
            ownerVigor?.Amount ?? 0);
        if (hpToHeal > 0 && ownerVigor is not null)
        {
            await PowerCmd.ModifyAmount(choiceContext, ownerVigor, -hpToHeal, owner.Creature, null);
            await CreatureCmd.Heal(newFigment.Creature, hpToHeal);
        }

        // Done!
        await imagineTimerTask;
        MainFile.Logger.Info($"Finished adding {newFigment.GetType().Name} instance.");
    }

    private static bool TryFillSlotsWithoutRepositioning(ReadOnlySpan<Figment> figments, Span<int> slotBuffer,
        int slotsAvailable)
    {
        var head = 0; // next figment slot cannot be left of head (inclusive start)
        var setPrevious = false;

        for (var i = 0; i < figments.Length; i += 1)
            if (figments[i].CurrentSlotIndex is { } preexisting)
            {
                if (preexisting < head) return false;

                if (setPrevious)
                {
                    // Place halfway between head (inclusive) and preexisting (exclusive)
                    // If an odd number of slots is available, bias left
                    var gap = preexisting - head - 1;
                    if (gap < 0) return false;
                    slotBuffer[i - 1] = head + gap / 2;
                }

                setPrevious = false;
                slotBuffer[i] = preexisting;
                head = preexisting + 1;
            }
            else
            {
                if (setPrevious)
                {
                    // Two figments without preexisting positions shouldn't happen, so just place previous at head
                    slotBuffer[i - 1] = head;
                    head += 1;
                }

                setPrevious = true;
            }

        if (setPrevious)
        {
            // Analogous to above, but with slotsAvailable as the exclusive end
            var gap = slotsAvailable - head - 1;
            if (gap < 0) return false;
            slotBuffer[^1] = head + gap / 2;
        }

        return head <= slotsAvailable;
    }

    private static void FillCleanSlots(ReadOnlySpan<Figment> figments, Span<int> slotBuffer, int slotsAvailable)
    {
        var start = (slotsAvailable - figments.Length) / 2;
        for (var i = 0; i < figments.Length; i += 1) slotBuffer[i] = start + i;
    }
}