using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using Trimaw.Core.Commands;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Snacks;

public class CaramelCandies : SnackCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipHelper.ForManifest<Glam>();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1),
        new CardsVar(3)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var enchanter = new Enchanter<Glam>
        {
            CanPotentiallyEnchantPredicate = c =>
                c.EnergyCost.GetWithModifiers(CostModifiers.All) == DynamicVars.Energy.IntValue
        };
        await EnchantCmd.Manifest(choiceContext, Owner, PileType.Hand, enchanter, DynamicVars.Cards.IntValue);
    }
}