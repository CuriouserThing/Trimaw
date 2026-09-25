using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.SharedIntents;

public class FastenIntent(int amount) : LabeledFigmentIntent<Figment>
{
    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Buff;

    protected override string DefaultTipIconPath => Pathfinder.VanillaPower64<FastenPower>();

    protected override void FormatIntentLabel(LocString label, MoveContext<Figment> ctx)
    {
        label.Add(new PowerVar<FastenPower>(ctx.TransformAmount(amount)));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<Figment> ctx)
    {
        desc.Add(new PowerVar<FastenPower>(amount));
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<Figment> ctx, PlayerChoiceContext choiceCtx)
    {
        await PowerCmd.Apply<FastenPower>(choiceCtx, ctx.PetOwner.Creature, ctx.TransformAmount(amount),
            ctx.MoveUser.Creature, null);
        return FigmentMoveResult.Success;
    }
}