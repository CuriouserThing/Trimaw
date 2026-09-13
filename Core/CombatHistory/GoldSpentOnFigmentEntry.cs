using MegaCrit.Sts2.Core.Entities.Players;
using Trimaw.Core.Models.Monsters;

namespace Trimaw.Core.CombatHistory;

public class GoldSpentOnFigmentEntry(Player player, int goldAmount, Figment figment) : TrimawCombatHistoryEntry(player)
{
    public int GoldAmount { get; } = goldAmount;

    public Figment Figment { get; } = figment;
}