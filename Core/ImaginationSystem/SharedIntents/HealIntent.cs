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

    private decimal CalculateHeal(MoveContext<Figment> ctx)
    {
        var threshold = ctx.MoveUser.OwnerHpThreshold;
        var current = ctx.PetOwner.Creature.CurrentHp;
        return Math.Min(ctx.TransformAmount(amount), threshold - current);
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<Figment> ctx)
    {
        var heal = CalculateHeal(ctx);
        label.Add(new HealVar(heal));
        label.Add("IfCapped", heal < amount);
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<Figment> ctx)
    {
        desc.Add(new HealVar(amount));
        desc.Add("Threshold", ctx.MoveUser.OwnerHpThreshold);
    }

    protected override bool CanPerform(MoveContext<Figment> ctx, out Creature? target)
    {
        // Turn only if healing player
        target = ctx.MoveUser.OwnerHpThreshold > ctx.PetOwner.Creature.CurrentHp
            ? ctx.PetOwner.Creature
            : null;
        return ctx.PetOwner.Creature.IsAlive;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<Figment> ctx, PlayerChoiceContext choiceCtx)
    {
        await ImaginationCmd.RefreshHp(ctx.MoveUser, (int)ctx.TransformAmount(amount));
        return FigmentMoveResult.Success;
    }
}