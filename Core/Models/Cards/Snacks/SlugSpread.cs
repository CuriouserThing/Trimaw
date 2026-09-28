using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace Trimaw.Core.Models.Cards.Snacks;

public class SlugSpread : SnackCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Momentum>()
        .Concat(HoverTipFactory.FromEnchantment<Slither>());

    protected override IEnumerable<DynamicVar> CanonicalVars => [new(nameof(Momentum), 5)];

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(Momentum)].UpgradeValueBy(5);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var rng = Owner.RunState.Rng.CombatCardSelection;

        var handCheapest = PileType.Hand.GetPile(Owner).Cards
            .Where(c => c.Type == CardType.Attack)
            .GroupBy(c => c.EnergyCost.GetWithModifiers(CostModifiers.All))
            .OrderBy(g => g.Key)
            .FirstOrDefault();
        if (handCheapest is not null && rng.NextItem(handCheapest) is { } handCard)
        {
            handCard = handCard.CreateClone();
            if (handCard.Enchantment is not null) CardCmd.ClearEnchantment(handCard);
            CardCmd.Enchant<Momentum>(handCard, DynamicVars[nameof(Momentum)].BaseValue);
            await CardPileCmd.AddGeneratedCardToCombat(handCard, PileType.Hand, Owner);
        }

        var deckCostliest = PileType.Deck.GetPile(Owner).Cards
            .Where(c => c.Type == CardType.Attack)
            .GroupBy(c => c.EnergyCost.GetWithModifiers(CostModifiers.All))
            .OrderByDescending(g => g.Key)
            .FirstOrDefault();
        if (deckCostliest is not null && rng.NextItem(deckCostliest) is { } deckCard)
        {
            deckCard = deckCard.CreateClone();
            if (deckCard.Enchantment is not null) CardCmd.ClearEnchantment(deckCard);
            CardCmd.Enchant<Slither>(deckCard, 1);
            await CardPileCmd.AddGeneratedCardToCombat(deckCard, PileType.Hand, Owner);
        }
    }
}