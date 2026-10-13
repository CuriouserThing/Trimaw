using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Enchantments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Snacks;

public class HoneyBiscuit : SnackCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipHelper.ForManifest<Polished>();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var threshold = DynamicVars.Energy.BaseValue;
        var enchanter = new Enchanter<Polished>
        {
            CanPotentiallyEnchantPredicate = c =>
                c.Type is CardType.Attack or CardType.Skill &&
                c.EnergyCost.GetWithModifiers(CostModifiers.Local) >= threshold
        };
        await EnchantCmd.Manifest(choiceContext, Owner, PileType.Hand, enchanter);
    }
}