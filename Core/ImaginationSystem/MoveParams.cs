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
        Addend = other.Addend;
        Multiplier = other.Multiplier;
        EnemyTarget = other.EnemyTarget;
    }

    public decimal? Addend { get; init; }

    public decimal? Multiplier { get; init; }

    /// <summary>
    ///     Single target that a <see cref="FigmentTrigger" /> may be passing to a <see cref="FigmentIntent" />.
    /// </summary>
    public Creature? EnemyTarget { get; init; }

    public static MoveParams None { get; } = new();
}