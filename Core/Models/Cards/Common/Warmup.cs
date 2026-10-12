using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Enchantments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Common;

public class Warmup() : TrimawCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipHelper.ForManifest<ReallyHot>()
        .Concat([HoverTipFactory.FromPower<VigorPower>()]);

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3, ValueProp.Move),
        new(nameof(ReallyHot), 4)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(ReallyHot)].UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await TrimawCmd
            .Attack(this, cardPlay, DynamicVars.Damage.BaseValue)
            .Targeting(cardPlay.Target)
            .WithStaffAnimation()
            .Execute(choiceContext);

        var enchanter = new Enchanter<ReallyHot>(DynamicVars[nameof(ReallyHot)].IntValue);
        await TrimawCmd.Manifest(Owner, PileType.Hand, enchanter);
    }
}