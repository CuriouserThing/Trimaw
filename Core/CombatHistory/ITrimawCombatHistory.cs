namespace Trimaw.Core.CombatHistory;

public interface ITrimawCombatHistory
{
    public IReadOnlyList<TrimawCombatHistoryEntry> AllHistoryEntries { get; }

    public void AddHistoryEntry(TrimawCombatHistoryEntry entry);
}