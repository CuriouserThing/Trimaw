using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem;

public class PlaceholderIntent : LabeledFigmentIntent<Figment>
{
    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Unknown;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("construction");

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<Figment> ctx, PlayerChoiceContext choiceCtx)
    {
        await PowerCmd.Apply<StrengthPower>(choiceCtx, ctx.PetOwner.Creature, 1, ctx.MoveUser.Creature, null);
        return FigmentMoveResult.Success;
    }
}