using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class BigUglyThingAttackCIntent : LabeledFigmentIntent<BigUglyThingFigment>
{
    private const decimal Damage = 6;
    private const int Repeat = 2;
    private const int RepeatPer = 1;
    private const int Energy = 1;

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack5;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("circular_sawblade");

    protected override string GetAnimationId(MoveContext<BigUglyThingFigment> ctx)
    {
        return BigUglyThingFigment.AttackCId;
    }

    private static Creature? GetTarget(MoveContext ctx)
    {
        var highest = ctx.CombatState.HittableEnemies.Max(c => c.CurrentHp);
        return ctx.GetEnemyTarget(c => c.CurrentHp == highest);
    }

    private static int GetHitCount(MoveContext ctx)
    {
        var energySpent = CombatManager.Instance.History.Entries.OfType<EnergySpentEntry>()
            .Where(e => e.HappenedThisTurn(ctx.CombatState) && e.Actor == ctx.PetOwner.Creature)
            .Select(e => e.Amount)
            .Sum();
        return Repeat + energySpent * RepeatPer;
    }

    private static AttackCommand BuildCommand(MoveContext ctx, Creature? target, int hitCount)
    {
        var cmd = DamageCmd
            .Attack(ctx.TransformAmount(Damage))
            .FromFigment(ctx.MoveUser)
            .WithHitCount(hitCount);
        if (target is not null) cmd = cmd.Targeting(target);
        return cmd;
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<BigUglyThingFigment> ctx)
    {
        var target = GetTarget(ctx);
        var hitCount = GetHitCount(ctx);
        var cmd = BuildCommand(ctx, target, hitCount);
        FormatWithTargetedDamage(label, ctx, target, new DamageVar(ctx.TransformAmount(Damage), ValueProp.Move));
        FormatWithAttackHitCount(label, ctx, cmd, new RepeatVar(hitCount));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<BigUglyThingFigment> ctx)
    {
        desc.Add(new DamageVar(Damage, ValueProp.Move));
        desc.Add(new RepeatVar(Repeat));
        desc.Add(new DynamicVar(nameof(RepeatPer), RepeatPer));
        desc.Add(new EnergyVar(Energy));
    }

    protected override bool CanPerform(MoveContext<BigUglyThingFigment> ctx, out Creature? visualTarget)
    {
        visualTarget = ctx.GetEnemyTarget();
        return visualTarget is not null;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<BigUglyThingFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (GetTarget(ctx) is not { } target) return FigmentMoveResult.NoValidTarget;

        await BuildCommand(ctx, target, GetHitCount(ctx)).Execute(choiceCtx);
        return FigmentMoveResult.Success;
    }
}