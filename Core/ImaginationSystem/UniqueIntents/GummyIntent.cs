using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.SnackSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class GummyIntent : UnlabeledFigmentIntent<GummyFigment>
{
    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Buff;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("cookie");

    protected override bool CanPerform(MoveContext<GummyFigment> ctx, out Creature? target)
    {
        target = ctx.PetOwner.Creature;
        return ctx.PetOwner.Creature.IsAlive;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<GummyFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        var amount = (int)ctx.TransformAmount(1);
        for (var i = 0; i < amount; i += 1)
            await SnackCmd.PrepSpecific(choiceCtx, ctx.CombatState, Morsel.Bread, ctx.PetOwner);
        return FigmentMoveResult.Success;
    }
}