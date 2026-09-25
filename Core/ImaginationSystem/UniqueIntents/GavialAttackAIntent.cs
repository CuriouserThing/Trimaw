using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class GavialAttackAIntent : LabeledFigmentIntent<GavialFigment>
{
    private const decimal StrengthLoss = 5;

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Debuff;

    protected override string DefaultTipIconPath => Pathfinder.VanillaPower64<ManglePower>();

    protected override void FormatIntentLabel(LocString label, MoveContext<GavialFigment> ctx)
    {
        label.Add(new DynamicVar(nameof(StrengthLoss), ctx.TransformAmount(StrengthLoss)));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<GavialFigment> ctx)
    {
        desc.Add(new DynamicVar(nameof(StrengthLoss), StrengthLoss));
    }

    protected override bool CanPerform(MoveContext<GavialFigment> ctx, out Creature? visualTarget)
    {
        visualTarget = ctx.GetEnemyTarget();
        return visualTarget is not null;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<GavialFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (ctx.GetEnemyTarget() is not { } target) return FigmentMoveResult.NoValidTarget;

        await PowerCmd.Apply<ManglePower>(choiceCtx, target, ctx.TransformAmount(StrengthLoss), ctx.MoveUser.Creature,
            null);
        return FigmentMoveResult.Success;
    }
}