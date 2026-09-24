using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class ZimaIntent : LabeledFigmentIntent<ZimaFigment>
{
    private const decimal Damage = 5;
    private const decimal Aftershock = 2;

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack2;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("bear_face");

    protected override void FormatIntentLabel(LocString label, MoveContext<ZimaFigment> ctx)
    {
        FormatWithAnyCreatureDamage(label, ctx,
            new DamageVar(ctx.TransformAmount(Damage), ValueProp.Move));
        FormatWithAnyCreatureDamage(label, ctx,
            new DamageVar(nameof(Aftershock), ctx.TransformAmount(Aftershock), ValueProp.Move));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<ZimaFigment> ctx)
    {
        FormatWithAnyCreatureDamage(desc, ctx, new DamageVar(Damage, ValueProp.Move));
        FormatWithAnyCreatureDamage(desc, ctx, new DamageVar(nameof(Aftershock), Aftershock, ValueProp.Move));
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<ZimaFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        await DamageCmd
            .Attack(ctx.TransformAmount(Damage))
            .FromFigment(ctx.MoveUser)
            .TargetingRandomOpponents(ctx.CombatState)
            .Execute(choiceCtx);

        await DamageCmd
            .Attack(ctx.TransformAmount(Aftershock))
            .FromFigment(ctx.MoveUser)
            .TargetingAllOpponents(ctx.CombatState)
            .WithWaitBeforeHit(0.2f, 0.2f)
            .Execute(choiceCtx);

        return FigmentMoveResult.Success;
    }
}