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
    private static readonly DamageVar Damage = new(6, ValueProp.Move);
    private static readonly RepeatVar Repeat = new(2);
    private static readonly RepeatVar RepeatPer = new("RepeatPer", 1);
    private static readonly EnergyVar Energy = new(1);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack5;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("circular_sawblade");

    protected override string GetAnimationId(MoveContext<BigUglyThingFigment> ctx)
    {
        return BigUglyThingFigment.AttackCId;
    }

    private static Creature? GetTarget(MoveContext ctx)
    {
        var highest = ctx.CombatState.HittableEnemies.Max(c => c.CurrentHp);
        return ctx.GetTarget(c => c.CurrentHp == highest);
    }

    private static int GetHitCount(MoveContext ctx)
    {
        var energySpent = CombatManager.Instance.History.Entries.OfType<EnergySpentEntry>()
            .Where(e => e.HappenedThisTurn(ctx.CombatState) && e.Actor == ctx.PetOwner.Creature)
            .Select(e => e.Amount)
            .Sum();
        return Repeat.IntValue + energySpent * RepeatPer.IntValue;
    }

    private static AttackCommand BuildCommand(MoveContext ctx, Creature? target, int hitCount)
    {
        var cmd = DamageCmd
            .Attack(Damage.BaseValue)
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
        FormatWithTargetedDamage(label, ctx, target, Damage);
        FormatWithAttackHitCount(label, ctx, cmd, new RepeatVar(hitCount));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<BigUglyThingFigment> ctx)
    {
        desc.Add(Damage);
        desc.Add(Repeat);
        desc.Add(RepeatPer);
        desc.Add(Energy);
    }

    protected override bool CanPerform(MoveContext<BigUglyThingFigment> ctx, out Creature? target)
    {
        target = ctx.GetTarget();
        return target is not null;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<BigUglyThingFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (GetTarget(ctx) is not { } target) return FigmentMoveResult.NoValidTarget;

        await BuildCommand(ctx, target, GetHitCount(ctx)).Execute(choiceCtx);
        return FigmentMoveResult.Success;
    }
}