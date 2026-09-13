using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class BigUglyThingSummonIntent : UnlabeledFigmentIntent<BigUglyThingFigment>
{
    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Summon;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("wrench");

    protected override string GetAnimationId(MoveContext<BigUglyThingFigment> ctx)
    {
        return BigUglyThingFigment.SummonId;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<BigUglyThingFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        await Cmd.Wait(0.1f);
        ctx.MoveUser.ShiftToDeathbed();
        await ctx.MoveUser.Pop();
        await ImaginationCmd.Imagine<EunectesFigment>(choiceCtx, ctx.PetOwner);
        return FigmentMoveResult.Success;
    }
}