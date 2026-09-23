using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class CryingThiefIntent : LabeledFigmentIntent<CryingThiefFigment>
{
    private static readonly DynamicVar StrengthLoss = new(nameof(StrengthLoss), 1);
    private static readonly DynamicVar ExtraStrengthLoss = new(nameof(ExtraStrengthLoss), 1);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Debuff;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("coat");

    private static decimal GetAmount(MoveContext ctx)
    {
        var payment = ctx.GetAmount();
        return StrengthLoss.BaseValue + payment * ExtraStrengthLoss.BaseValue;
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<CryingThiefFigment> ctx)
    {
        label.Add(new DynamicVar(nameof(StrengthLoss), GetAmount(ctx)));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<CryingThiefFigment> ctx)
    {
        desc.Add(StrengthLoss);
        desc.Add(ExtraStrengthLoss);
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<CryingThiefFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        foreach (var enemy in ctx.CombatState.HittableEnemies)
            await PowerCmd.Apply<StrengthPower>(choiceCtx, enemy, -GetAmount(ctx), ctx.MoveUser.Creature, null);
        return FigmentMoveResult.Success;
    }
}