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
    private const decimal Damage = 10;
    private const decimal ExtraDamage = 2;
    private const int ExtraHp = 50;

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack4;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("harpoon_chain");

    private static Creature? GetTarget(MoveContext ctx)
    {
        var highest = ctx.CombatState.HittableEnemies.Max(c => c.CurrentHp);
        return ctx.GetEnemyTarget(c => c.CurrentHp == highest);
    }

    private static decimal GetDamage(Creature? target, MoveContext ctx)
    {
        var damage = Damage;
        if (target is not null)
        {
            var mult = target.MaxHp / ExtraHp;
            damage += ExtraDamage * mult;
        }

        return ctx.TransformAmount(damage);
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<RosaFigment> ctx)
    {
        var target = GetTarget(ctx);
        var damage = GetDamage(target, ctx);
        FormatWithTargetedDamage(label, ctx, target, new DamageVar(damage, ValueProp.Move));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<RosaFigment> ctx)
    {
        desc.Add(new DamageVar(Damage, ValueProp.Move));
        desc.Add(new ExtraDamageVar(ExtraDamage));
        desc.Add(new DynamicVar(nameof(ExtraHp), ExtraHp));
    }

    protected override bool CanPerform(MoveContext<RosaFigment> ctx, out Creature? visualTarget)
    {
        visualTarget = ctx.GetEnemyTarget();
        return visualTarget is not null;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<RosaFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (GetTarget(ctx) is not { } target) return FigmentMoveResult.NoValidTarget;

        await DamageCmd
            .Attack(GetDamage(target, ctx))
            .FromFigment(ctx.MoveUser)
            .Targeting(target)
            .Execute(choiceCtx);
        return FigmentMoveResult.Success;
    }
}