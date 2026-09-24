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

public class ShamareIntent : UnlabeledFigmentIntent<ShamareFigment>
{
    private static readonly PowerVar<VulnerablePower> Vulnerable = new(2);
    private static readonly PowerVar<WeakPower> Weak = new(2);
    private static readonly PowerVar<DoomPower> Doom = new(6);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Debuff;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("teddy_bear");

    protected override void FormatTipDescription(LocString desc, MoveContext<ShamareFigment> ctx)
    {
        desc.Add(Vulnerable);
        desc.Add(Weak);
        desc.Add(Doom);
    }

    protected override bool CanPerform(MoveContext<ShamareFigment> ctx, out Creature? target)
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

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<ShamareFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (ctx.GetTarget() is not { } target) return FigmentMoveResult.NoValidTarget;

        await Apply(ctx, choiceCtx, target, Vulnerable);
        await Apply(ctx, choiceCtx, target, Weak);
        await Apply(ctx, choiceCtx, target, Doom);
        return FigmentMoveResult.Success;
    }
}