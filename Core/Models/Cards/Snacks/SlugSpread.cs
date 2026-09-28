using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
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
        await Transform<Momentum>(PileType.Hand, false, DynamicVars[nameof(Momentum)].IntValue);
        await Transform<Slither>(PileType.Draw, true, 1);
    }

    private async Task Transform<T>(PileType pileType, bool max, int amount) where T : EnchantmentModel
    {
        var enchantment = ModelDb.Enchantment<T>();
        var pile = pileType.GetPile(Owner).Cards
            .Where(c => c.Type == CardType.Attack && enchantment.CanEnchant(c))
            .ToArray();
        var pileExtreme = max ? pile.Max(GetCost) : pile.Min(GetCost);
        var pileExtremes = pile.Where(c => GetCost(c) == pileExtreme).ToArray();
        var rng = Owner.RunState.Rng.CombatCardSelection;
        if (pileExtremes.Length > 0 && rng.NextItem(pileExtremes) is { } pileCard)
        {
            var clone = pileCard.CreateClone();
            if (clone.Enchantment is not null) CardCmd.ClearEnchantment(clone);
            CardCmd.Enchant<T>(clone, amount);
            await CardCmd.Transform(pileCard, clone);
        }
    }

    private static int GetCost(CardModel card)
    {
        return card.EnergyCost.GetWithModifiers(CostModifiers.All);
    }
}