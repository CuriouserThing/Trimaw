using MegaCrit.Sts2.Core.Entities.Creatures;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.ImaginationSystem;

public class MoveParams
{
    /// <summary>
    ///     General-purpose number that a <see cref="TriggerTalent" /> may be passing to a <see cref="FigmentIntent" />.
    ///     Must be positive.
    /// </summary>
    public decimal? Amount { get; init; } = null;

    /// <summary>
    ///     Single target that a <see cref="TriggerTalent" /> may be passing to a <see cref="FigmentIntent" />.
    /// </summary>
    public Creature? Target { get; init; } = null;

    public static MoveParams None { get; } = new();
}