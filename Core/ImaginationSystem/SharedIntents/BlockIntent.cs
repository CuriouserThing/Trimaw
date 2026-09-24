using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.SharedIntents;

public class BlockIntent(int amount) : LabeledFigmentIntent<Figment>
{
    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Defend;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("shield");

    protected override void FormatIntentLabel(LocString label, MoveContext<Figment> ctx)
    {
        FormatWithBlock(label, ctx, ctx.PetOwner.Creature, new BlockVar(ctx.TransformAmount(amount), ValueProp.Move));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<Figment> ctx)
    {
        FormatWithBlock(desc, ctx, ctx.PetOwner.Creature, new BlockVar(amount, ValueProp.Move));
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<Figment> ctx, PlayerChoiceContext choiceCtx)
    {
        await CreatureCmd.GainBlock(ctx.PetOwner.Creature, new BlockVar(ctx.TransformAmount(amount), ValueProp.Move),
            null);
        return FigmentMoveResult.Success;
    }
}