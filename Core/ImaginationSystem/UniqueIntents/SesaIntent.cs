using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class SesaIntent : LabeledFigmentIntent<SesaFigment>
{
    private static readonly DynamicVar Amount = new("Amount", 4);

    private static DamageVar Damage => ModelDb.Power<TheBombPower>().DynamicVars.Damage;

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack5;

    protected override string DefaultTipIconPath => Pathfinder.VanillaPower64<TheBombPower>();

    protected override void FormatIntentLabel(LocString label, MoveContext<SesaFigment> ctx)
    {
        label.Add(Damage);
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<SesaFigment> ctx)
    {
        desc.Add(Damage);
        desc.Add(Amount);
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<SesaFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        await PowerCmd.Apply<TheBombPower>(
            choiceCtx,
            ctx.PetOwner.Creature,
            Amount.BaseValue,
            ctx.MoveUser.Creature,
            null);
        return FigmentMoveResult.Success;
    }
}