using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Models.Powers.Triggers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class FlintIntent : LabeledFigmentIntent<FlintFigment>
{
    private const decimal Damage = MahuizzotiaTrigger.DamageThreshold;

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack3;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("boxing_glove");

    protected override void FormatIntentLabel(LocString label, MoveContext<FlintFigment> ctx)
    {
        FormatWithAnyCreatureDamage(label, ctx,
            new DamageVar(ctx.TransformAmount(Damage), ValueProp.Unpowered | ValueProp.Move));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<FlintFigment> ctx)
    {
        desc.Add(new DamageVar(Damage, ValueProp.Unpowered | ValueProp.Move));
    }

    protected override bool CanPerform(MoveContext<FlintFigment> ctx, out Creature? visualTarget)
    {
        visualTarget = ctx.GetEnemyTarget();
        return visualTarget is not null;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<FlintFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (ctx.GetEnemyTarget() is not { } target) return FigmentMoveResult.NoValidTarget;

        await CreatureCmd.Damage(choiceCtx, target, new DamageVar(Damage, ValueProp.Unpowered | ValueProp.Move),
            ctx.MoveUser.Creature);
        return FigmentMoveResult.Success;
    }
}