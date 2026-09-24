using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.ImaginationSystem;

namespace Trimaw.Core.Models.Powers.Talents;

public class ArtsAssimilationTalent : FigmentTalent
{
    private const string StepKey = "Step";

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new(StepKey, 5)];

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card.Owner != Figment.PetOwner ||
            card.Pile?.Type != PileType.Hand ||
            card.Enchantment is null ||
            Amount >= 100) return;

        Flash();
        var total = Math.Min(100, Amount + DynamicVars[StepKey].BaseValue);
        var step = total - Amount;

        PlayerChoiceContext choiceCtx = CombatState.CurrentSide == CombatSide.Enemy || !LocalContext.NetId.HasValue
            ? new BlockingPlayerChoiceContext()
            : new HookPlayerChoiceContext(Figment.PetOwner, LocalContext.NetId.Value, GameActionType.Combat);
        await PowerCmd.ModifyAmount(choiceCtx, this, step, Figment.Creature, null);
    }

    protected internal override MoveParams ModifyMoveParams(MoveParams moveParams, bool dryRun)
    {
        if (dryRun) return moveParams;

        var rng = CombatState.RunState.Rng.Niche; // NOTE: nothing better at the moment, but keep this in consideration 
        var crit = Amount > rng.NextInt(0, 100);
        return crit ? new MoveParams(moveParams) { Multiplier = 2 * moveParams.Multiplier } : moveParams;
    }

    protected internal override async Task AfterModifyingMoveParams(MoveParams originalParams, MoveParams newParams)
    {
        await PowerCmd.Remove(this);
    }
}