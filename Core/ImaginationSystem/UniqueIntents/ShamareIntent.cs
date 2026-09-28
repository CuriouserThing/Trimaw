using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class ShamareIntent : UnlabeledFigmentIntent<ShamareFigment>
{
    private static readonly PowerVar<DemisePower> Demise = new(1);
    private static readonly PowerVar<DoomPower> Doom = new(13);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Debuff;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("teddy_bear");

    protected override void FormatTipDescription(LocString desc, MoveContext<ShamareFigment> ctx)
    {
        desc.Add(Demise);
        desc.Add(Doom);
    }

    protected override bool CanPerform(MoveContext<ShamareFigment> ctx, out Creature? visualTarget)
    {
        visualTarget = ctx.GetEnemyTarget();
        return visualTarget is not null;
    }

    private static async Task Apply<T>(MoveContext ctx, PlayerChoiceContext choiceCtx, Creature target,
        PowerVar<T> powerVar) where T : PowerModel
    {
        await PowerCmd.Apply<T>(choiceCtx, target, ctx.TransformAmount(powerVar.BaseValue), ctx.MoveUser.Creature,
            null);
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<ShamareFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (ctx.GetEnemyTarget() is not { } target) return FigmentMoveResult.NoValidTarget;

        await Apply(ctx, choiceCtx, target, Demise);
        await Apply(ctx, choiceCtx, target, Doom);
        return FigmentMoveResult.Success;
    }
}