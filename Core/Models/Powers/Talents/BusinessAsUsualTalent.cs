using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Powers;
using Trimaw.Core.CombatHistory;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Talents;

public class BusinessAsUsualTalent : FigmentTalent
{
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string Icon64Path => Pathfinder.NotoEmoji64("money_bag");
    public override string Icon256Path => Pathfinder.NotoEmoji256("money_bag");

    protected internal override MoveParams ModifyMoveParams(MoveParams moveParams)
    {
        var oldAmount = moveParams.Amount ?? 0;
        var payment = Figment.PetOwner.Gold / Amount;
        return new MoveParams(moveParams) { Amount = oldAmount + payment };
    }

    protected internal override async Task AfterModifyingMoveParams(MoveParams originalParams, MoveParams newParams)
    {
        var payment = (int)((newParams.Amount ?? 0) - (originalParams.Amount ?? 0));
        if (payment <= 0) return;
        await PlayerCmd.LoseGold(payment, Figment.PetOwner, GoldLossType.Spent);
        var historyEntry = new GoldSpentOnFigmentEntry(Figment.PetOwner, payment, Figment);
        MainFile.CombatManagerFactory.GetOrCreate(Figment.PetOwner).AddHistoryEntry(historyEntry);
    }
}