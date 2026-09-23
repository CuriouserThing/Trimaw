using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class GavialAttackAIntent : LabeledFigmentIntent<GavialFigment>
{
    private static readonly DynamicVar StrengthLoss = new("StrengthLoss", 10);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Debuff;

    protected override string DefaultTipIconPath => Pathfinder.VanillaPower64<ManglePower>();

    protected override void FormatIntentLabel(LocString label, MoveContext<GavialFigment> ctx)
    {
        label.Add(StrengthLoss);
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<GavialFigment> ctx)
    {
        desc.Add(StrengthLoss);
    }

    protected override bool CanPerform(MoveContext<GavialFigment> ctx, out Creature? target)
    {
        target = ctx.GetTarget();
        return target is not null;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<GavialFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (ctx.GetTarget() is not { } target) return FigmentMoveResult.NoValidTarget;

        await PowerCmd.Apply<ManglePower>(choiceCtx, target, StrengthLoss.BaseValue, ctx.MoveUser.Creature, null);
        await PowerCmd.Apply<HuntingBuddyTalent>(choiceCtx, ctx.MoveUser.Creature, 1, ctx.MoveUser.Creature, null);
        return FigmentMoveResult.Success;
    }
}