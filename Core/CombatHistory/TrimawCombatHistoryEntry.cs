using MegaCrit.Sts2.Core.Entities.Players;

namespace Trimaw.Core.CombatHistory;

public abstract class TrimawCombatHistoryEntry(Player player)
{
    public Player Player { get; } = player;
}