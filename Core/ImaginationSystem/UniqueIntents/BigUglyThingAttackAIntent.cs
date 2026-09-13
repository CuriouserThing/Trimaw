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

public class BigUglyThingAttackAIntent : LabeledFigmentIntent<BigUglyThingFigment>
{
    private static readonly DamageVar Damage = new(10, ValueProp.Move);
    private static readonly DynamicVar Vulnerable = new PowerVar<VulnerablePower>(1);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack2;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("jigsaw_puzzle_piece");

    private static Creature? GetTarget(MoveContext ctx)
    {
        var highest = ctx.CombatState.HittableEnemies.Max(c => c.CurrentHp);
        return ctx.GetTarget(c => c.CurrentHp == highest);
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<BigUglyThingFigment> ctx)
    {
        FormatWithTargetedDamage(label, ctx, GetTarget(ctx), Damage);
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<BigUglyThingFigment> ctx)
    {
        desc.Add(Damage);
        desc.Add(Vulnerable);
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

        await DamageCmd
            .Attack(Damage.BaseValue)
            .FromFigment(ctx.MoveUser)
            .Targeting(target)
            .Execute(choiceCtx);
        await PowerCmd.Apply<VulnerablePower>(choiceCtx, target, Vulnerable.BaseValue, ctx.MoveUser.Creature, null);
        return FigmentMoveResult.Success;
    }
}