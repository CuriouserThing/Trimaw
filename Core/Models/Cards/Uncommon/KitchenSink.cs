using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Enchantments;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class KitchenSink() : TrimawCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        HoverTipFactory.FromEnchantment<ReallyHot>()
            .Concat(HoverTipFactory.FromEnchantment<ReallyCold>())
            .Concat([HoverTipFactory.Static(StaticHoverTip.Prep), HoverTipFactory.Static(StaticHoverTip.Water)]);

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(nameof(ReallyHot), 4),
        new(nameof(ReallyCold), 4)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(ReallyHot)].UpgradeValueBy(2);
        DynamicVars[nameof(ReallyCold)].UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardsToTransform = GetCardsToTransform();
        if (cardsToTransform is not null)
        {
            CardModel[] transformedCards =
            [
                // ReallyCold first so it appears visually to the right on the transform pop-up (hot/cold spigots)
                Clone<ReallyCold>(cardsToTransform[1], DynamicVars[nameof(ReallyCold)].BaseValue),
                Clone<ReallyHot>(cardsToTransform[0], DynamicVars[nameof(ReallyHot)].BaseValue)
            ];
            var transforms = cardsToTransform.Zip(transformedCards, (a, b) => new CardTransformation(a, b));
            await CardCmd.Transform(transforms, null);
        }

        await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Water, Owner);
    }

    private List<CardModel>? GetCardsToTransform()
    {
        var reallyHot = ModelDb.Enchantment<ReallyHot>();
        var reallyCold = ModelDb.Enchantment<ReallyCold>();
        var cards = PileType.Draw.GetPile(Owner).Cards
            .Where(c => reallyHot.CanEnchant(c) && reallyCold.CanEnchant(c))
            .ToList()
            .UnstableShuffle(Owner.RunState.Rng.CombatCardSelection);

        const int toTake = 2;
        switch (cards.Count)
        {
            case < toTake:
                return null;
            case toTake:
                return cards;
            case > toTake:
                // Favor cards that aren't already enchanted (don't bother if only one is unenchanted)
                var unenchantedCards = cards.Where(c => c.Enchantment is null).Take(toTake).ToList();
                return unenchantedCards.Count == toTake ? unenchantedCards : [.. cards.Take(toTake)];
        }
    }

    private static CardModel Clone<T>(CardModel card, decimal amount) where T : EnchantmentModel
    {
        var clone = card.CreateClone();
        CardCmd.ClearEnchantment(clone);
        CardCmd.Enchant<T>(clone, amount);
        return clone;
    }
}