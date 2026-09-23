using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class FattyIntent : LabeledFigmentIntent<FattyFigment>
{
    private static readonly PowerVar<PlatingPower> Plating = new(2);
    private static readonly PowerVar<PlatingPower> ExtraPlating = new("ExtraPlatingPower", 1);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Buff;

    protected override string DefaultTipIconPath => Pathfinder.VanillaPower64<PlatingPower>();

    private static decimal GetAmount(MoveContext ctx)
    {
        var payment = ctx.GetAmount();
        return Plating.BaseValue + payment * ExtraPlating.BaseValue;
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<FattyFigment> ctx)
    {
        label.Add(new PowerVar<PlatingPower>(GetAmount(ctx)));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<FattyFigment> ctx)
    {
        desc.Add(Plating);
        desc.Add(ExtraPlating);
    }

    protected override bool CanPerform(MoveContext<FattyFigment> ctx, out Creature? target)
    {
        target = ctx.PetOwner.Creature;
        return ctx.PetOwner.Creature.IsAlive;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<FattyFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        await PowerCmd.Apply<PlatingPower>(choiceCtx, ctx.PetOwner.Creature, GetAmount(ctx), ctx.MoveUser.Creature,
            null);
        return FigmentMoveResult.Success;
    }
}