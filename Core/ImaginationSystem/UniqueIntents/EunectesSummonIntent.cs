using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class EunectesSummonIntent : UnlabeledFigmentIntent<EunectesFigment>
{
    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Summon;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("fire_shield");

    protected override string GetAnimationId(MoveContext<EunectesFigment> ctx)
    {
        return EunectesFigment.SummonId;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<EunectesFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        // The animation is timed such that she's now transparent + off-screen, so turn off visibility right away
        if (ctx.MoveUser.Creature.GetCreatureNode() is { } nFigment) nFigment.Visible = false;

        await ctx.MoveUser.Pop();
        await Cmd.Wait(0.25f);
        await ImaginationCmd.Imagine<BigUglyThingFigment>(choiceCtx, ctx.PetOwner);
        return FigmentMoveResult.Success;
    }
}