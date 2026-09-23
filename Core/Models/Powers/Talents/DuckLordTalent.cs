using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Powers;
using Trimaw.Core.CombatHistory;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Talents;

public class DuckLordTalent : FigmentTalent
{
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string Icon64Path => Pathfinder.NotoEmoji64("money_with_wings");
    public override string Icon256Path => Pathfinder.NotoEmoji256("money_with_wings");

    private decimal? GetPayment()
    {
        var gold = Figment.PetOwner.Gold;
        return gold < Amount ? null : gold / Amount;
    }

    public override bool ShouldDie(Creature creature)
    {
        if (creature != Owner) return true;

        return GetPayment() is null;
    }

    public override async Task AfterPreventingDeath(Creature creature)
    {
        Flash();
        var payment = Math.Min(GetPayment() ?? 1, Owner.MaxHp);
        await CreatureCmd.Heal(creature, payment);
        await PlayerCmd.LoseGold(payment, Figment.PetOwner, GoldLossType.Spent);
        var historyEntry = new GoldSpentOnFigmentEntry(Figment.PetOwner, (int)payment, Figment);
        MainFile.CombatManagerFactory.GetOrCreate(Figment.PetOwner).AddHistoryEntry(historyEntry);
    }
}