using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class BigUglyThingAttackBIntent : LabeledFigmentIntent<BigUglyThingFigment>
{
    private const decimal Damage = 5;

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack2;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("stun_grenade");

    protected override string GetAnimationId(MoveContext<BigUglyThingFigment> ctx)
    {
        return BigUglyThingFigment.AttackBId;
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<BigUglyThingFigment> ctx)
    {
        FormatWithAnyCreatureDamage(label, ctx, new DamageVar(ctx.TransformAmount(Damage), ValueProp.Move));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<BigUglyThingFigment> ctx)
    {
        desc.Add(new DamageVar(Damage, ValueProp.Move));
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<BigUglyThingFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        await DamageCmd
            .Attack(ctx.TransformAmount(Damage))
            .FromFigment(ctx.MoveUser)
            .TargetingAllOpponents(ctx.CombatState)
            .Execute(choiceCtx);
        foreach (var enemy in ctx.CombatState.HittableEnemies.Where(c => c.HasPower<VulnerablePower>()))
            await CreatureCmd.Stun(enemy);
        return FigmentMoveResult.Success;
    }
}