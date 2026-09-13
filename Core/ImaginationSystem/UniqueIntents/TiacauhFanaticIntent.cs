using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class TiacauhFanaticIntent : LabeledFigmentIntent<TiacauhFanaticFigment>
{
    private static readonly DynamicVar Weak = new PowerVar<WeakPower>(3);
    private static readonly DynamicVar Frail = new PowerVar<FrailPower>(3);
    private static readonly DynamicVar Poison = new PowerVar<PoisonPower>(3);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Debuff;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("triple_claws");

    protected override void FormatIntentLabel(LocString label, MoveContext<TiacauhFanaticFigment> ctx)
    {
        // Helps if they're all the same number, but the world doesn't end if we change that
        // This is different from Shamare where they're never gonna be the same number
        label.Add(Weak);
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<TiacauhFanaticFigment> ctx)
    {
        desc.Add(Weak);
        desc.Add(Frail);
        desc.Add(Poison);
    }

    protected override bool CanPerform(MoveContext<TiacauhFanaticFigment> ctx, out Creature? target)
    {
        target = ctx.GetTarget();
        return target is not null;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<TiacauhFanaticFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (ctx.GetTarget() is not { } target) return FigmentMoveResult.NoValidTarget;

        await PowerCmd.Apply<WeakPower>(choiceCtx, target, Weak.BaseValue, ctx.MoveUser.Creature, null);
        await PowerCmd.Apply<FrailPower>(choiceCtx, target, Frail.BaseValue, ctx.MoveUser.Creature, null);
        await PowerCmd.Apply<PoisonPower>(choiceCtx, target, Poison.BaseValue, ctx.MoveUser.Creature, null);
        return FigmentMoveResult.Success;
    }
}