using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.GodotControls;
using Trimaw.Core.Hooks;
using Trimaw.Core.Models.Cards;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core.Commands;

public static class SnackCmd
{
    private static IPrepManager ManagerForPlayer(Player player)
    {
        return MainFile.CombatManagerFactory.GetOrCreate(player);
    }

    public static async Task<SnackCard?> PrepSpecific(
        PlayerChoiceContext choiceContext,
        ICombatState? combatState,
        Morsel morsel,
        Player player,
        PileType snackPileType = PileType.Hand,
        CardPilePosition snackPilePosition = CardPilePosition.Bottom)
    {
        var snackResult = ManagerForPlayer(player).AddMorsel(morsel);
        return await Process(choiceContext, combatState, morsel, snackResult, player, snackPileType, snackPilePosition);
    }

    public static async Task<SnackCard?> PrepRandom(
        PlayerChoiceContext choiceContext,
        ICombatState? combatState,
        Player player,
        PileType snackPileType = PileType.Hand,
        CardPilePosition snackPilePosition = CardPilePosition.Bottom)
    {
        var manager = ManagerForPlayer(player);
        var morsel = manager.GenerateRandomMorsel();
        var snackResult = manager.AddMorsel(morsel);
        return await Process(choiceContext, combatState, morsel, snackResult, player, snackPileType, snackPilePosition);
    }

    public static async Task<SnackCard?> PrepPrevious(
        PlayerChoiceContext choiceContext,
        ICombatState? combatState,
        Player player,
        PileType snackPileType = PileType.Hand,
        CardPilePosition snackPilePosition = CardPilePosition.Bottom)
    {
        // PreviousMorsel will be null if:
        // 1) the starting relic has been removed (?) or otherwise negated (???), and
        // 2) the first prep effect is an add-previous effect
        // This is an extreme corner case not worth mentioning to player.
        // But it's hypothetically not impossible (?), so just add random in that case.

        var manager = ManagerForPlayer(player);
        var morsel = manager.PreviousMorsel ?? manager.GenerateRandomMorsel();
        var snackResult = manager.AddMorsel(morsel);
        return await Process(choiceContext, combatState, morsel, snackResult, player, snackPileType, snackPilePosition);
    }

    private static async Task<SnackCard?> Process(
        PlayerChoiceContext choiceContext,
        ICombatState? combatState,
        Morsel morsel,
        SnackResult? result,
        Player player,
        PileType snackPileType,
        CardPilePosition snackPilePosition)
    {
        if (combatState is null) return null;

        await TrimawHook.BeforeMorselPrepped(player.RunState, combatState, choiceContext, player, morsel);

        if (player.Creature.GetCreatureNode() is { } nCreature &&
            MorselStockWrapper.NCreatureTable[nCreature] is { } stockControl)
        {
            stockControl.AddMorsel(morsel);
            await Cmd.CustomScaledWait(0.4f, 0.7f);
            if (result is not null)
            {
                stockControl.PopSnack(result);
                await Cmd.CustomScaledWait(0.4f, 0.7f);
            }
        }

        SnackCard? card = null;
        if (result is not null)
        {
            card = result.CreatedSnack.ToCard(combatState, player);
            await CardPileCmd.AddGeneratedCardToCombat(card, snackPileType, player, snackPilePosition);
        }

        await TrimawHook.AfterMorselPrepped(player.RunState, combatState, choiceContext, player, morsel, card);
        return card;
    }
}