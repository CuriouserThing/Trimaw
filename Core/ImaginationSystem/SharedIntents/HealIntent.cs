using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.SharedIntents;

public class HealIntent(int amount) : LabeledFigmentIntent<Figment>
{
    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Heal;

    protected override string DefaultTipIconPath => Pathfinder.VanillaPower64<RegenPower>();

    protected override void FormatIntentLabel(LocString label, MoveContext<Figment> ctx)
    {
        label.Add(new HealVar(ctx.TransformAmount(amount)));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<Figment> ctx)
    {
        desc.Add(new HealVar(amount));
    }

    private static Creature? GetTarget(MoveContext ctx)
    {
        return ctx.PetOwner.GetFigments()
            .Select(f => f.Creature)
            .Where(c => c.CurrentHp < c.MaxHp)
            .OrderBy(c => c.CurrentHp)
            .FirstOrDefault();
    }

    protected override bool CanPerform(MoveContext<Figment> ctx, out Creature? visualTarget)
    {
        var target = GetTarget(ctx);
        visualTarget = target;
        return target is not null;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<Figment> ctx, PlayerChoiceContext choiceCtx)
    {
        if (GetTarget(ctx) is not { } target) return FigmentMoveResult.NoValidTarget;
        await CreatureCmd.Heal(target, ctx.TransformAmount(amount));
        return FigmentMoveResult.Success;
    }
}