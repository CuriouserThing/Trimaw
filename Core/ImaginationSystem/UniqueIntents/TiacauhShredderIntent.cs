using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class TiacauhShredderIntent : LabeledFigmentIntent<TiacauhShredderFigment>
{
    private const decimal Damage = 7;
    private const decimal ExtraDamage = 5;

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack3;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("cleaver");

    private static bool TargetReceivesExtra(Creature target)
    {
        return target.HasPower<VulnerablePower>() || target.HasPower<FrailPower>();
    }

    private static decimal GetDamage(MoveContext ctx)
    {
        var damage = Damage;
        if (ctx.CombatState.HittableEnemies.All(TargetReceivesExtra)) damage += ExtraDamage;
        return ctx.TransformAmount(damage);
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<TiacauhShredderFigment> ctx)
    {
        FormatWithAnyCreatureDamage(label, ctx, new DamageVar(GetDamage(ctx), ValueProp.Move));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<TiacauhShredderFigment> ctx)
    {
        desc.Add(new DynamicVar(nameof(Damage), Damage));
        desc.Add(new DynamicVar(nameof(ExtraDamage), ExtraDamage));
    }

    protected override bool CanPerform(MoveContext<TiacauhShredderFigment> ctx, out Creature? visualTarget)
    {
        visualTarget = ctx.GetEnemyTarget();
        return visualTarget is not null;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<TiacauhShredderFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (ctx.GetEnemyTarget() is not { } target) return FigmentMoveResult.NoValidTarget;

        await DamageCmd
            .Attack(GetDamage(ctx))
            .FromFigment(ctx.MoveUser)
            .Targeting(target)
            .Execute(choiceCtx);
        return FigmentMoveResult.Success;
    }
}