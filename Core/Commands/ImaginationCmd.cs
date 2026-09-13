using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Monsters;

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

    public static async Task ImagineGachaPull(PlayerChoiceContext choiceContext, Player owner)
    {
        var manager = MainFile.CombatManagerFactory.GetOrCreate(owner);
        var taggedFigment = manager.GachaPullFigment(owner);
        await manager.ImagineFigment(choiceContext, owner, taggedFigment);
    }

    public static async Task Imagine(PlayerChoiceContext choiceContext, Player owner, FigmentFilter filter)
    {
        var manager = MainFile.CombatManagerFactory.GetOrCreate(owner);
        var taggedFigment = manager.FilterFigment(owner, filter);
        await manager.ImagineFigment(choiceContext, owner, taggedFigment);
    }

    public static async Task Imagine<T>(PlayerChoiceContext choiceContext, Player owner) where T : Figment
    {
        var manager = MainFile.CombatManagerFactory.GetOrCreate(owner);
        var taggedFigment = TaggedFigment.FromModel<T>();
        await manager.ImagineFigment(choiceContext, owner, taggedFigment);
    }

    internal static async Task Imagine<T>(PlayerChoiceContext choiceContext, Player owner,
        FigmentSlotMap slotMap) where T : Figment
    {
        var monster = ModelDb.Monster<T>().ToMutable();
        var creature = owner.Creature.CombatState?.CreateCreature(monster, owner.Creature.Side, null);
        if (creature is null || monster is not T newFigment)
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
        //      We distribute the popping figment's HP to live figments *here* without physically healing them,
        //      since we need to wait until they're in physical position before triggering the heal animation
        var allFigments = figmentBuffer.AsSpan();
        var allFigmentSlots = slotBuffer.AsSpan();
        var liveFigments = allFigments[liveRange];
        var liveFigmentSlots = allFigmentSlots[liveRange];
        var liveFigmentHpMap = DistributeHp(liveFigments,
            oldestPopping ? oldestFigment.Creature.CurrentHp : 0);
        liveFigments.Sort((x, y) =>
        {
            // Sort by hit priority from back (low) to front (high)
            var hpDelta = liveFigmentHpMap[x] - liveFigmentHpMap[y];
            if (hpDelta != 0) return hpDelta;
            return (x.Timestamp ?? 0) - (y.Timestamp ?? 0);
        });

        // Assign slots to live figments, avoiding repositions if possible
        // Keep the popping figment at its current slot regardless
        var slotsAvailable = slotMap.Slots.Count;
        var repositionNeeded = !TryFillSlotsWithoutRepositioning(liveFigments, liveFigmentSlots, slotsAvailable);
        if (repositionNeeded) FillCleanSlots(liveFigments, liveFigmentSlots, slotsAvailable);
        if (oldestPopping) allFigmentSlots[0] = oldestFigment.CurrentSlotIndex ?? 0;

        // Physically place *all* figments into assigned slots (even the popping figment),
        // creating reposition tweens as needed
        const float shiftDuration = 0.50f;
        if (owner.Creature.GetCreatureNode() is { } nOwner)
        {
            var parent = nOwner.GetParent<CanvasItem>();
            parent.YSortEnabled = true;
            nOwner.YSortEnabled = true;

            for (var i = 0; i < allFigments.Length; i += 1)
                allFigments[i].SetSlot(allFigmentSlots[i], slotMap, nOwner, shiftDuration);
        }

        if (oldestPopping)
        {
            // Wait for 1) the popping figment to use and finish its move for visual clarity, and
            // 2) live figment repositions to finish so the +HP animation plays at the correct position
            await oldestFigment.UseMove(choiceContext, MoveParams.None, TriggerKind.Pop);
            if (oldestFigment.NoAnimationPending) await oldestFigment.Pop();
            if (repositionNeeded) await Cmd.CustomScaledWait(shiftDuration, shiftDuration);

            // *Now* heal live figments up to their calculated HP values 
            for (var i = 1; i < figmentBuffer.Length; i += 1)
            {
                if (oldestFigment.NoAnimationPending) await oldestFigment.Pop();
                var figment = figmentBuffer[i];
                var heal = liveFigmentHpMap[figment] - figment.Creature.CurrentHp;
                if (heal > 0) await CreatureCmd.Heal(figment.Creature, heal);
            }

            // One last check to see if the animation has finished
            if (oldestFigment.NoAnimationPending) await oldestFigment.Pop();
        }

        // Finish new figment initialization
        // NOTE: if there's a figment power that uses a hook that the above move/heals trigger,
        // the choice to put it here *is* mechanically significant -- food for thought
        await newFigment.SetMoveAndPowers(choiceContext);

        // Done!
        if (oldestPopping) await oldestFigment.Pop();
        await imagineTimerTask;
        MainFile.Logger.Info($"Finished adding {newFigment.GetType().Name} instance.");
    }

    public static async Task RefreshHp(Figment healer, int amount)
    {
        if (amount < 1)
        {
            MainFile.Logger.Error($"Can only refresh a positive amount of HP (attempted to refresh {amount}.");
            return;
        }

        var petOwner = healer.PetOwner;
        var threshold = healer.OwnerHpThreshold;
        var current = petOwner.Creature.CurrentHp;
        var playerHeal = Math.Min(amount, threshold - current);
        if (playerHeal > 0) await CreatureCmd.Heal(petOwner.Creature, playerHeal);

        var figments = petOwner.GetFigments().ToArray();
        var hpMap = DistributeHp(figments, amount - playerHeal);
        foreach (var figment in figments)
        {
            var heal = hpMap[figment] - figment.Creature.CurrentHp;
            if (heal > 0) await CreatureCmd.Heal(figment.Creature, heal);
        }
    }

    private static Dictionary<Figment, int> DistributeHp(ReadOnlySpan<Figment> figments, int hp)
    {
        var figmentHpMap = new Dictionary<Figment, int>();
        foreach (var figment in figments) figmentHpMap.Add(figment, figment.Creature.CurrentHp);

        var figmentsToShareWith = figments.Length;
        while (figmentsToShareWith > 0 && hp >= figmentsToShareWith)
        {
            var cap = hp / figmentsToShareWith; // truncated for even HP sharing
            figmentsToShareWith = 0; // recount figments
            foreach (var figment in figments)
            {
                var gap = figment.Creature.MaxHp - figmentHpMap[figment];
                var heal = Math.Min(cap, gap);
                if (heal < gap) figmentsToShareWith += 1; // can still take more HP
                figmentHpMap[figment] += heal;
                hp -= heal;
            }
        }

        // There may be a miniscule amount of HP left, so just parcel it out as able in figment order
        foreach (var figment in figments)
        {
            var cap = hp;
            var gap = figment.Creature.MaxHp - figmentHpMap[figment];
            var heal = Math.Min(cap, gap);
            figmentHpMap[figment] += heal;
            hp -= heal;
        }

        return figmentHpMap;
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