using Trimaw.Core.Animation;
using Trimaw.Core.CombatHistory;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core;

/// <summary>
///     Interface for holding and processing all of this mod's unique combat state.
/// </summary>
public interface ITrimawCombatManager :
    ITrimawCombatHistory,
    IPrepManager,
    IFigmentFilterer,
    ISuperSpecializer,
    ICeobeAnimator
{
    public int ConcurrentFigmentLimit => 2; // for now, always 2
}