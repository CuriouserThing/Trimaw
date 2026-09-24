using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class TiacauhFanaticIntent : LabeledFigmentIntent<TiacauhFanaticFigment>
{
    private static readonly PowerVar<WeakPower> Weak = new(3);
    private static readonly PowerVar<FrailPower> Frail = new(3);
    private static readonly PowerVar<PoisonPower> Poison = new(3);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Debuff;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("triple_claws");

    protected override void FormatIntentLabel(LocString label, MoveContext<TiacauhFanaticFigment> ctx)
    {
        // Helps if they're all the same number, but the world doesn't end if we change that
        // This is different from Shamare where they're never gonna be the same number
        var amount = ctx.TransformAmount(Weak.BaseValue);
        label.Add("Amount", amount);
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

    private static async Task Apply<T>(MoveContext ctx, PlayerChoiceContext choiceCtx, Creature target,
        PowerVar<T> powerVar) where T : PowerModel
    {
        await PowerCmd.Apply<T>(choiceCtx, target, ctx.TransformAmount(powerVar.BaseValue), ctx.MoveUser.Creature,
            null);
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<TiacauhFanaticFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (ctx.GetTarget() is not { } target) return FigmentMoveResult.NoValidTarget;

        await Apply(ctx, choiceCtx, target, Weak);
        await Apply(ctx, choiceCtx, target, Frail);
        await Apply(ctx, choiceCtx, target, Poison);
        return FigmentMoveResult.Success;
    }
}