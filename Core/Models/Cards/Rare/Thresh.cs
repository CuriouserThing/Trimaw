using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Commands;

namespace Trimaw.Core.Models.Cards.Rare;

public class Thresh() : TrimawCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    private const string HpThresholdKey = "HpThreshold";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(16),
        new ExtraDamageVar(1),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(GetMultiplier),
        new(HpThresholdKey, 15)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(4);
        DynamicVars[HpThresholdKey].UpgradeValueBy(-5);
    }

    private static decimal GetMultiplier(CardModel card, Creature? creature)
    {
        if (creature?.MaxHp is not { } maxHp) return 0;

        var mult = maxHp / card.DynamicVars[HpThresholdKey].IntValue;
        return mult;
    }

    public override int ModifyAttackHitCount(AttackCommand attack, int hitCount)
    {
        if (attack.CardPlay?.Target is not { } target) return hitCount;

        if (target.Block > 0 || target.GetPowerAmount<ArtifactPower>() > 0) hitCount += 1;
        return hitCount;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await TrimawCmd
            .Attack(this, cardPlay, DynamicVars.CalculatedDamage.Calculate(cardPlay.Target))
            .Targeting(cardPlay.Target)
            .WithSpearAnimation()
            .Execute(choiceContext);
    }
}