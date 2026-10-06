using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Commands;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class SliceAndDice() : TrimawCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Prep),
        HoverTipFactory.Static(StaticHoverTip.Meat),
        HoverTipFactory.Static(StaticHoverTip.Pepper)
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5, ValueProp.Move),
        new RepeatVar(2)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await TrimawCmd
            .Attack(this, cardPlay, DynamicVars.Damage.BaseValue)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .Targeting(cardPlay.Target)
            .WithKnifeAnimation()
            .Execute(choiceContext);

        await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Meat, Owner);
        await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Pepper, Owner);
    }
}