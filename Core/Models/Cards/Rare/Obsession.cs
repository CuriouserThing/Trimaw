using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Enchantments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Rare;

public class Obsession() : TrimawCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipHelper.ForManifest<Hyperfixated>();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var enchanter = new Enchanter<Hyperfixated>
        {
            PreventedFromEnchantingPredicate = c => c.EnergyCost.GetWithModifiers(CostModifiers.None) != 1
        };
        await EnchantCmd.Manifest(choiceContext, Owner, PileType.Hand, enchanter);

        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
}