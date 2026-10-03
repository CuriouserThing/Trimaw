using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Animation;

namespace Trimaw.Core.Commands;

/// <summary>
///     Commands that don't fit elsewhere.
/// </summary>
public static class TrimawCmd
{
    public static Task MakeSuperSpecial(CardModel card)
    {
        // It's not impossible to envision some fx here, if it's in hand
        MainFile.Logger.Info($"Making {card} Super Special.");
        MainFile.CombatManagerFactory.GetOrCreate(card.Owner).MakeSuperSpecial(card);
        return Task.CompletedTask;
    }

    public static async Task TransformIntoEnchantedCopies<T>(Player owner, IEnumerable<CardModel> candidates, int cardCount, int enchantAmount = 1) where T : EnchantmentModel
    {
        var enchantment = ModelDb.Enchantment<T>();
        var rng = owner.RunState.Rng.CombatCardSelection;
        var cards = candidates.Where(enchantment.CanEnchant).TakeRandom(cardCount, rng).ToArray();
        var clones = new CardModel[cards.Length];
        for (int i = 0; i < cards.Length; i += 1)
        {
            var clone = cards[i].CreateClone();
            CardCmd.ClearEnchantment(clone);
            CardCmd.Enchant<T>(clone, enchantAmount);
            clones[i] = clone;
        }

        var transforms = cards.Zip(clones, (a, b) => new CardTransformation(a, b));
        await CardCmd.Transform(transforms, null);
    }

    public static TrimawAttackCommand Attack(CardModel card, CardPlay? cardPlay, decimal damagePerHit)
    {
        return new TrimawAttackCommand(card, cardPlay, damagePerHit);
    }
}