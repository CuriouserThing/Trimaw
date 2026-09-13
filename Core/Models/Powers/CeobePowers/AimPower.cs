using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class AimPower : TrimawPower
{
    private const string AdditionalDamagePctKey = "AdditionalDamagePct";
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string Icon64Path => Pathfinder.NotoEmoji64("direct_hit");
    public override string Icon256Path => Pathfinder.NotoEmoji256("direct_hit");
    protected override IEnumerable<DynamicVar> CanonicalVars => [new(AdditionalDamagePctKey, 25)];

    public decimal DamageMult => 1 + DynamicVars[AdditionalDamagePctKey].BaseValue / 100M;

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props,
        Creature? dealer,
        CardModel? cardSource, CardPlay? cardPlay)
    {
        if (target is not null && cardSource?.CurrentTarget is { } cardTarget && target != cardTarget)
            // Deal normal damage to enemies hit by some AoE/aftershock if they weren't *the* enemy that had a card dragged to them.
            return 1;
        if (dealer == Owner && cardSource?.TargetType == TargetType.AnyEnemy)
            // Otherwise, to make sure damage preview works, multiply all damage from cards that target an enemy.
            return DamageMult;
        return 1;
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer,
        DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer != Owner || target != cardSource?.CurrentTarget) return;

        await PowerCmd.Decrement(this);
    }
}