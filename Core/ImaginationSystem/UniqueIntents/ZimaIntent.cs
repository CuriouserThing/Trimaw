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
    private static readonly DamageVar PrimaryDamage = new(5, ValueProp.Move);
    private static readonly DamageVar AftershockDamage = new("Aftershock", 2, ValueProp.Move);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack2;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("bear_face");

    private void Format(LocString str, MoveContext ctx)
    {
        FormatWithMultiCreatureDamage(str, ctx, PrimaryDamage);
        FormatWithMultiCreatureDamage(str, ctx, AftershockDamage);
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<ZimaFigment> ctx)
    {
        Format(label, ctx);
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<ZimaFigment> ctx)
    {
        Format(desc, ctx);
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<ZimaFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        await DamageCmd
            .Attack(PrimaryDamage.BaseValue)
            .FromFigment(ctx.MoveUser)
            .TargetingRandomOpponents(ctx.CombatState)
            .Execute(choiceCtx);

        await DamageCmd
            .Attack(AftershockDamage.BaseValue)
            .FromFigment(ctx.MoveUser)
            .TargetingAllOpponents(ctx.CombatState)
            .WithWaitBeforeHit(0.2f, 0.2f)
            .Execute(choiceCtx);

        return FigmentMoveResult.Success;
    }
}