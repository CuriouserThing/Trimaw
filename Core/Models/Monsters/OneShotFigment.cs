using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.Models.Monsters;

/// <summary>
///     Basic <see cref="Figment" /> with <see cref="ImaginaryShieldPower" /> and a single intent.
/// </summary>
public abstract class OneShotFigment<TTrigger, TTalent>() : AnyShotFigment<TTrigger, TTalent>(1)
    where TTrigger : FigmentTrigger
    where TTalent : FigmentTalent
{
    protected sealed override FigmentIntent GetFigmentIntent(int idx)
    {
        return GetFigmentIntent();
    }

    protected abstract FigmentIntent GetFigmentIntent();
}