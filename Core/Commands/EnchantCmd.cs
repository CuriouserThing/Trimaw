using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Hooks;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Commands;

public static class EnchantCmd
{
    public static async Task<CardModel> GenerateCard(PlayerChoiceContext choiceContext, Player owner,
        Enchanter enchanter, CardPoolModel? cardPool = null)
    {
        var combatState = owner.Creature.CombatState;
        ArgumentNullException.ThrowIfNull(combatState);

        cardPool ??= owner.Character.CardPool;
        var pool = cardPool
            .GetUnlockedCards(owner.UnlockState, owner.RunState.CardMultiplayerConstraint)
            .Where(enchanter.CanEnchantCloneOf);
        var rng = owner.RunState.Rng.CombatCardGeneration;
        var card = CardFactory.GetForCombat(owner, pool, 1, rng).FirstOrDefault();
        if (card is null) throw new Exception($"No card found in pool for enchanter {enchanter}.");

        await Enchant(combatState, choiceContext, card, enchanter);
        return card;
    }

    public static async Task<IReadOnlyList<CardModel>> GenerateCards(PlayerChoiceContext choiceContext, Player owner,
        Enchanter enchanter, int cardCount, CardPoolModel? cardPool = null, bool getDistinctCards = true)
    {
        var combatState = owner.Creature.CombatState;
        ArgumentNullException.ThrowIfNull(combatState);
        if (cardCount < 1)
        {
            MainFile.Logger.Error($"Attempted to manifest a non-positive total of {cardCount} cards.");
            return [];
        }

        cardPool ??= owner.Character.CardPool;
        var pool = cardPool
            .GetUnlockedCards(owner.UnlockState, owner.RunState.CardMultiplayerConstraint)
            .Where(enchanter.CanEnchantCloneOf);
        var rng = owner.RunState.Rng.CombatCardGeneration;
        var cards = getDistinctCards
            ? CardFactory.GetDistinctForCombat(owner, pool, cardCount, rng).ToArray()
            : CardFactory.GetForCombat(owner, pool, cardCount, rng).ToArray();
        if (cards.Length != cardCount)
            throw new Exception($"Not enough cards found in pool for enchanter {enchanter}.");

        foreach (var card in cards) await Enchant(combatState, choiceContext, card, enchanter);
        return cards;
    }

    public static async Task Manifest(PlayerChoiceContext choiceContext, Player owner, PileType pileType,
        Enchanter enchanter, int cardCount = 1)
    {
        if (cardCount < 1)
        {
            MainFile.Logger.Error($"Attempted to manifest a non-positive total of {cardCount} cards.");
            return;
        }

        var enchanters = new Enchanter[cardCount];
        Array.Fill(enchanters, enchanter);
        await Manifest(choiceContext, owner, pileType, enchanters);
    }

    public static async Task Manifest(PlayerChoiceContext choiceContext, Player owner, PileType pileType,
        IReadOnlyList<Enchanter> enchanters)
    {
        var combatState = owner.Creature.CombatState;
        ArgumentNullException.ThrowIfNull(combatState);

        var pileCards = pileType.GetPile(owner).Cards.ToHashSet();
        var rng = owner.RunState.Rng;
        var transformOriginals = new List<CardModel>(enchanters.Count);
        var transformClones = new List<CardModel>(enchanters.Count);
        var generatedCards = new List<CardModel>(enchanters.Count);
        foreach (var enchanter in enchanters)
            if (rng.CombatCardSelection.NextItem(pileCards.Where(enchanter.CanEnchantCloneOf)) is { } card)
            {
                pileCards.Remove(card);
                transformOriginals.Add(card);

                var clone = card.CreateClone();
                await Enchant(combatState, choiceContext, clone, enchanter);
                transformClones.Add(clone);
            }
            else
            {
                var pool = owner.Character.CardPool
                    .GetUnlockedCards(owner.UnlockState, owner.RunState.CardMultiplayerConstraint)
                    .Where(enchanter.CanEnchantCloneOf);
                card = CardFactory.GetForCombat(owner, pool, 1, rng.CombatCardGeneration).FirstOrDefault();
                if (card is null)
                {
                    MainFile.Logger.Error(
                        $"No card to generate for character {owner.Character} with enchanter {enchanter}");
                }
                else
                {
                    await Enchant(combatState, choiceContext, card, enchanter);
                    generatedCards.Add(card);
                }
            }

        if (transformOriginals.Count > 0)
        {
            var transforms = transformOriginals.Zip(transformClones, (a, b) => new CardTransformation(a, b));
            await CardCmd.Transform(transforms, null);
        }

        if (generatedCards.Count > 0)
        {
            var pilePosition = pileType == PileType.Draw ? CardPilePosition.Random : CardPilePosition.Bottom;
            var addResults = await CardPileCmd.AddGeneratedCardsToCombat(generatedCards, pileType, owner, pilePosition);
            CardCmd.PreviewCardPileAdd(addResults);
        }
    }

    private static async Task Enchant(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card,
        Enchanter enchanter)
    {
        enchanter = await TrimawHook.ModifyEnchanter(combatState, choiceContext, card, enchanter);
        enchanter.Enchant(card);
    }
}