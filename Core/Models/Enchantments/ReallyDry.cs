using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class ReallyDry : ReallyXEnchantment
{
    public override string Icon64Path => Pathfinder.NotoEmoji64("high_voltage_sign");

    protected override async Task Operate()
    {
        var candidates = Card.CombatState?.HittableEnemies;
        if (candidates is null || candidates.Count == 0) return;

        var target = Card.Owner.RunState.Rng.CombatTargets.NextItem(candidates);
        if (target is null) return;

        var choiceCtx = GetChoiceContext();
        var damage = new DamageVar(Amount, ValueProp.Unpowered);
        var dealer = Card.Owner.Creature;
        await CreatureCmd.Damage(choiceCtx, target, damage, dealer, null, null);
    }
}