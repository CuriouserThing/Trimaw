using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using Trimaw.Core.Commands;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Snacks;

public class SlugSpread : SnackCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipHelper.ForManifest<Momentum>()
        .Concat(HoverTipFactory.FromEnchantment<Slither>());

    protected override IEnumerable<DynamicVar> CanonicalVars => [new(nameof(Momentum), 5)];

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(Momentum)].UpgradeValueBy(5);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var momentumEnchanter = new Enchanter<Momentum>(DynamicVars[nameof(Momentum)].IntValue)
        {
            CanPotentiallyEnchantPredicate = c => c.EnergyCost.GetWithModifiers(CostModifiers.All) == 1
        };
        await TrimawCmd.Manifest(Owner, PileType.Hand, momentumEnchanter);

        var slitherEnchanter = new Enchanter<Slither>
        {
            CanPotentiallyEnchantPredicate = c => c.EnergyCost.GetWithModifiers(CostModifiers.All) == 2
        };
        await TrimawCmd.Manifest(Owner, PileType.Draw, slitherEnchanter);
    }
}