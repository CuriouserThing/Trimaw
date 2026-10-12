using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Animation;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Commands;

/// <summary>
///     Commands that don't fit elsewhere.
/// </summary>
public static class TrimawCmd
{
    public static async Task Manifest(Player owner, PileType pileType, IEnchanter enchanter, int cardCount = 1)
    {
        if (cardCount < 1)
        {
            MainFile.Logger.Error($"Attempted to manifest a non-positive total of {cardCount} cards.");
            return;
        }

        var enchanters = new IEnchanter[cardCount];
        Array.Fill(enchanters, enchanter);
        await Manifest(owner, pileType, enchanters);
    }

    public static async Task Manifest(Player owner, PileType pileType, IReadOnlyList<IEnchanter> enchanters)
    {
        var pileCards = pileType.GetPile(owner).Cards.ToHashSet();
        var rng = owner.RunState.Rng;
        var transformOriginals = new List<CardModel>(enchanters.Count);
        var transformClones = new List<CardModel>(enchanters.Count);
        var generatedCards = new List<CardModel>(enchanters.Count);
        foreach (var enchanter in enchanters)
            if (rng.CombatCardSelection.NextItem(pileCards.Where(enchanter.CanEnchant)) is { } card)
            {
                pileCards.Remove(card);
                transformOriginals.Add(card);

                var clone = card.CreateClone();
                CardCmd.ClearEnchantment(clone);
                enchanter.Enchant(clone);
                transformClones.Add(clone);
            }
            else
            {
                var pool = owner.Character.CardPool
                    .GetUnlockedCards(owner.UnlockState, owner.RunState.CardMultiplayerConstraint)
                    .Where(enchanter.CanEnchant);
                card = CardFactory.GetForCombat(owner, pool, 1, rng.CombatCardGeneration).FirstOrDefault();
                if (card is null)
                {
                    MainFile.Logger.Error(
                        $"No card to generate for character {owner.Character} with enchanter {enchanter}");
                }
                else
                {
                    enchanter.Enchant(card);
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
            await CardPileCmd.AddGeneratedCardsToCombat(generatedCards, pileType, owner, pilePosition);
        }
    }

    public static TrimawAttackCommand Attack(CardModel card, CardPlay? cardPlay, decimal damagePerHit)
    {
        return new TrimawAttackCommand(card, cardPlay, damagePerHit);
    }
}