using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.SnackSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class FluorescentSlugIntent : UnlabeledFigmentIntent<FluorescentSlugFigment>
{
    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Buff;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("knife_fork");

    protected override bool CanPerform(MoveContext<FluorescentSlugFigment> ctx, out Creature? target)
    {
        target = ctx.PetOwner.Creature;
        return true;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<FluorescentSlugFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        var amount = (int)ctx.TransformAmount(1);
        for (var i = 0; i < amount; i += 1)
            await SnackCmd.PrepSpecific(choiceCtx, ctx.CombatState, Morsel.Shrooms, ctx.PetOwner);
        return FigmentMoveResult.Success;
    }
}