using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class GopnikIntent : LabeledFigmentIntent<GopnikFigment>
{
    private static readonly DamageVar Damage = new(12, ValueProp.Move);
    private static readonly ExtraDamageVar ExtraDamage = new(6);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack4;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("chart_with_upwards_trend");

    private static decimal GetAmount(MoveContext ctx)
    {
        var payment = ctx.GetAmount();
        return Damage.BaseValue + payment * ExtraDamage.BaseValue;
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<GopnikFigment> ctx)
    {
        FormatWithAnyCreatureDamage(label, ctx, new DamageVar(GetAmount(ctx), ValueProp.Move));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<GopnikFigment> ctx)
    {
        // For the tip, show uncalculated damage to better convey what the move is doing
        desc.Add(Damage);
        desc.Add(ExtraDamage);
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<GopnikFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        await DamageCmd
            .Attack(GetAmount(ctx))
            .FromFigment(ctx.MoveUser)
            .TargetingRandomOpponents(ctx.CombatState)
            .Execute(choiceCtx);
        return FigmentMoveResult.Success;
    }
}