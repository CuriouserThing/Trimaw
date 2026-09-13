using Trimaw.Core.Models.Monsters;

namespace Trimaw.Core.ImaginationSystem;

public class FigmentMoveResult
{
    public static readonly FigmentMoveResult Success = new();

    /// <summary>
    ///     Could not perform move because of a type mismatch between the <see cref="Figment" /> itself and its
    ///     <see cref="FigmentIntent{T}" />.
    /// </summary>
    public static readonly FigmentMoveResult MismatchedFigmentType = new();

    /// <summary>
    ///     Move fizzled because there was no valid target to use it on.
    /// </summary>
    public static readonly FigmentMoveResult NoValidTarget = new();

    private FigmentMoveResult()
    {
    }
}