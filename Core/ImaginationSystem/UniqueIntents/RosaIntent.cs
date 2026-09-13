using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class RosaIntent : LabeledFigmentIntent<RosaFigment>
{
    private static readonly DamageVar Damage = new(10, ValueProp.Move);
    private static readonly ExtraDamageVar ExtraDamage = new(2);
    private static readonly DynamicVar ExtraHp = new("ExtraHp", 50);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack4;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("harpoon_chain");

    private static Creature? GetTarget(MoveContext ctx)
    {
        var highest = ctx.CombatState.HittableEnemies.Max(c => c.CurrentHp);
        return ctx.GetTarget(c => c.CurrentHp == highest);
    }

    private static DamageVar GetDamage(Creature? target)
    {
        var damage = Damage.BaseValue;
        if (target is not null)
        {
            var mult = target.MaxHp / ExtraHp.IntValue;
            damage += ExtraDamage.BaseValue * mult;
        }

        return new DamageVar(damage, ValueProp.Move);
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<RosaFigment> ctx)
    {
        // For the label, show the calculated damage
        var target = GetTarget(ctx);
        var damage = GetDamage(target);
        FormatWithTargetedDamage(label, ctx, target, damage);
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<RosaFigment> ctx)
    {
        // For the tip, show uncalculated damage to better convey what the move is doing
        desc.Add(Damage);
        desc.Add(ExtraDamage);
        desc.Add(ExtraHp);
    }

    protected override bool CanPerform(MoveContext<RosaFigment> ctx, out Creature? target)
    {
        target = ctx.GetTarget();
        return target is not null;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<RosaFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (GetTarget(ctx) is not { } target) return FigmentMoveResult.NoValidTarget;

        var damage = GetDamage(target);
        await DamageCmd
            .Attack(damage.BaseValue)
            .FromFigment(ctx.MoveUser)
            .Targeting(target)
            .Execute(choiceCtx);
        return FigmentMoveResult.Success;
    }
}