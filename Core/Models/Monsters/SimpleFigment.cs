using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.Models.Monsters;

/// <summary>
///     <see cref="OneShotFigment{TTalent}" /> with a parameterless intent.
/// </summary>
public abstract class SimpleFigment<TTalent, TIntent> : OneShotFigment<TTalent>
    where TTalent : FigmentTalentPower
    where TIntent : FigmentIntent, new()
{
    protected sealed override TIntent GetFigmentIntent()
    {
        return new TIntent();
    }
}