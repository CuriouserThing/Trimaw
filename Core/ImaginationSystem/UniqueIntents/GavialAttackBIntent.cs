using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class GavialAttackBIntent : LabeledFigmentIntent<GavialFigment>
{
    private static readonly DamageVar Damage = new(8, ValueProp.Move);
    private static readonly RepeatVar Repeat = new(2);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack3;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("axe_swing");

    protected override string GetAnimationId(MoveContext<GavialFigment> ctx)
    {
        return GavialFigment.AttackBId;
    }

    private static AttackCommand BuildCommand(MoveContext ctx)
    {
        return DamageCmd
            .Attack(Damage.BaseValue)
            .FromFigment(ctx.MoveUser)
            .WithHitCount(Repeat.IntValue)
            .TargetingRandomOpponents(ctx.CombatState);
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<GavialFigment> ctx)
    {
        FormatWithAnyCreatureDamage(label, ctx, Damage);
        FormatWithAttackHitCount(label, ctx, BuildCommand(ctx), Repeat);
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<GavialFigment> ctx)
    {
        desc.Add(Damage);
        desc.Add(Repeat);
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<GavialFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        await BuildCommand(ctx).Execute(choiceCtx);
        return FigmentMoveResult.Success;
    }
}