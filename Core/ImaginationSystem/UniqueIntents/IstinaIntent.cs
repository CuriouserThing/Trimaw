using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class IstinaIntent : LabeledFigmentIntent<IstinaFigment>
{
    private static readonly EnergyVar Energy = new(2);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Buff;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("books");

    protected override void FormatIntentLabel(LocString label, MoveContext<IstinaFigment> ctx)
    {
        label.Add(Energy);
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<IstinaFigment> ctx)
    {
        desc.Add(Energy);
    }

    protected override bool CanPerform(MoveContext<IstinaFigment> ctx, out Creature? target)
    {
        target = ctx.PetOwner.Creature;
        return true;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<IstinaFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        await PlayerCmd.GainEnergy(Energy.BaseValue, ctx.PetOwner);
        return FigmentMoveResult.Success;
    }
}