using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class FlintIntent : LabeledFigmentIntent<FlintFigment>
{
    private static readonly DamageVar Damage = new(10, ValueProp.Move);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack3;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("boxing_glove");

    protected override void FormatIntentLabel(LocString label, MoveContext<FlintFigment> ctx)
    {
        FormatWithAnyCreatureDamage(label, ctx, Damage);
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<FlintFigment> ctx)
    {
        desc.Add(Damage);
    }

    protected override bool CanPerform(MoveContext<FlintFigment> ctx, out Creature? target)
    {
        target = ctx.GetTarget();
        return target is not null;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<FlintFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (ctx.GetTarget() is not { } target) return FigmentMoveResult.NoValidTarget;

        var damage = Math.Max(Damage.BaseValue, ctx.GetAmount());
        await DamageCmd
            .Attack(damage)
            .FromFigment(ctx.MoveUser)
            .Targeting(target)
            .Execute(choiceCtx);
        return FigmentMoveResult.Success;
    }
}