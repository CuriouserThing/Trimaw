using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.CombatHistory;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.SharedTalents;

public class EotGoldTrigger : SingleTriggerTalent
{
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string Icon64Path => Pathfinder.NotoEmoji64("money_bag");
    public override string Icon256Path => Pathfinder.NotoEmoji256("money_bag");

    protected internal override decimal? EstimatedAmount => GetPayment();

    private decimal? GetPayment()
    {
        var gold = Figment.PetOwner.Gold;
        return gold < Amount ? null : gold / Amount;
    }

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player || GetPayment() is not { } payment) return;

        await PlayerCmd.LoseGold(payment, Figment.PetOwner, GoldLossType.Spent);
        var historyEntry = new GoldSpentOnFigmentEntry(Figment.PetOwner, (int)payment, Figment);
        MainFile.CombatManagerFactory.GetOrCreate(Figment.PetOwner).AddHistoryEntry(historyEntry);

        Figment.MarkForPopping();
        await TriggerMove(choiceContext, new MoveParams { Amount = payment });
        await Figment.Pop();
    }
}