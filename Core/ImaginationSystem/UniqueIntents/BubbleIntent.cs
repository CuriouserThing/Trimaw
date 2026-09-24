using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class BubbleIntent : LabeledFigmentIntent<BubbleFigment>
{
    private const decimal Thorns = 2;

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Buff;

    protected override string DefaultTipIconPath => Pathfinder.VanillaPower64<ThornsPower>();

    protected override void FormatIntentLabel(LocString label, MoveContext<BubbleFigment> ctx)
    {
        label.Add(new PowerVar<ThornsPower>(ctx.TransformAmount(Thorns)));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<BubbleFigment> ctx)
    {
        desc.Add(new PowerVar<ThornsPower>(Thorns));
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<BubbleFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        await PowerCmd.Apply<ThornsPower>(choiceCtx, ctx.PetOwner.Creature, ctx.TransformAmount(Thorns),
            ctx.MoveUser.Creature, null);
        return FigmentMoveResult.Success;
    }
}