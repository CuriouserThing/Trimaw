using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Commands;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class Dumbbells() : TrimawCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<VigorPower>(),
        HoverTipFactory.FromPower<StrengthPower>()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<StrengthPower>(6),
        new DamageVar(4, ValueProp.Move),
        new RepeatVar(2)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(StrengthPower)].UpgradeValueBy(3);
        DynamicVars.Damage.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (PowerCmd.FindExistingInstanceForStacking(ModelDb.Power<VigorPower>(), creature, creature) is { } vigorPower)
        {
            var vigor = vigorPower.Amount;
            var strength = Math.Min(DynamicVars[nameof(StrengthPower)].IntValue, vigor);
            if (vigor == strength)
                await PowerCmd.Remove(vigorPower);
            else
                await PowerCmd.ModifyAmount(choiceContext, vigorPower, -strength, creature, this);
            await PowerCmd.Apply<StrengthPower>(choiceContext, creature, strength, creature, this);
        }

        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await TrimawCmd
            .Attack(this, cardPlay, DynamicVars.Damage.BaseValue)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .Targeting(cardPlay.Target)
            .WithAxeAnimation()
            .WithTimescale(1.5f)
            .Execute(choiceContext);
    }
}