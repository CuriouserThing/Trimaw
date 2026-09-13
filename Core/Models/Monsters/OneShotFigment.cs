using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.Models.Monsters;

/// <summary>
///     Basic <see cref="Figment" /> with <see cref="ImaginaryShieldPower" />, <see cref="EvanescentPower" />, and a single
///     intent.
/// </summary>
public abstract class OneShotFigment<TTalent>() : AnyShotFigment<TTalent>(1) where TTalent : FigmentTalentPower
{
    protected sealed override FigmentIntent GetFigmentIntent(int idx)
    {
        return GetFigmentIntent();
    }

    protected abstract FigmentIntent GetFigmentIntent();
}