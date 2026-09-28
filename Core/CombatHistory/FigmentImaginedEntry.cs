using MegaCrit.Sts2.Core.Entities.Players;
using Trimaw.Core.Models.Monsters;

namespace Trimaw.Core.CombatHistory;

public class FigmentImaginedEntry(Player player, Figment figment) : TrimawCombatHistoryEntry(player)
{
    public Figment Figment { get; } = figment;
}