using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class ShamareIntent : UnlabeledFigmentIntent<ShamareFigment>
{
    private static readonly DynamicVar Vulnerable = new PowerVar<VulnerablePower>(2);
    private static readonly DynamicVar Weak = new PowerVar<WeakPower>(2);
    private static readonly DynamicVar Doom = new PowerVar<DoomPower>(6);

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

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<ShamareFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (ctx.GetTarget() is not { } target) return FigmentMoveResult.NoValidTarget;

        await PowerCmd.Apply<VulnerablePower>(choiceCtx, target, Vulnerable.BaseValue, ctx.MoveUser.Creature, null);
        await PowerCmd.Apply<WeakPower>(choiceCtx, target, Weak.BaseValue, ctx.MoveUser.Creature, null);
        await PowerCmd.Apply<DoomPower>(choiceCtx, target, Doom.BaseValue, ctx.MoveUser.Creature, null);
        return FigmentMoveResult.Success;
    }
}