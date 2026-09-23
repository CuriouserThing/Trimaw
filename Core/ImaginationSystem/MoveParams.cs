using MegaCrit.Sts2.Core.Entities.Creatures;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.ImaginationSystem;

public class MoveParams
{
    public MoveParams()
    {
    }

    public MoveParams(MoveParams other)
    {
        Amount = other.Amount;
        Target = other.Target;
    }

    /// <summary>
    ///     General-purpose number that a <see cref="FigmentTrigger" /> may be passing to a <see cref="FigmentIntent" />.
    ///     Must be positive.
    /// </summary>
    public decimal? Amount { get; init; }

    /// <summary>
    ///     Single target that a <see cref="FigmentTrigger" /> may be passing to a <see cref="FigmentIntent" />.
    /// </summary>
    public Creature? Target { get; init; }

    public static MoveParams None { get; } = new();
}