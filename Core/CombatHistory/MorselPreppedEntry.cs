using MegaCrit.Sts2.Core.Entities.Players;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core.CombatHistory;

public class MorselPreppedEntry(Player player, Morsel morsel) : TrimawCombatHistoryEntry(player)
{
    public Morsel Morsel { get; } = morsel;
}