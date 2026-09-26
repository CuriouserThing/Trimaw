using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class EunectesBuildIntent : UnlabeledFigmentIntent<EunectesFigment>
{
    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Unknown;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("nut_and_bolt");

    protected override bool CanPerform(MoveContext<EunectesFigment> ctx, out Creature? visualTarget)
    {
        visualTarget = ctx.PetOwner.Creature; // turn toward player, even though this doesn't target them per se
        return true;
    }

    protected override Task<FigmentMoveResult> OnPerform(MoveContext<EunectesFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        // Fake move
        return Task.FromResult(FigmentMoveResult.Success);
    }
}