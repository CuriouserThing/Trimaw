using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Commands;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class BangBumpPow() : TrimawCard(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    private const string BreadAmountKey = "BreadAmount";

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Prep),
        HoverTipFactory.Static(StaticHoverTip.Bread)
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(9, ValueProp.Move),
        new RepeatVar(3),
        new(BreadAmountKey, 3)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        var cmd = await TrimawCmd
            .Attack(this, cardPlay, DynamicVars.Damage.BaseValue)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .Targeting(cardPlay.Target)
            .WithKnifeAnimation()
            .Execute(choiceContext);

        if (cmd.Results.SelectMany(r => r).Any(r => r.WasTargetKilled))
            for (var i = 0; i < DynamicVars[BreadAmountKey].IntValue; i += 1)
                await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Bread, Owner);
    }
}