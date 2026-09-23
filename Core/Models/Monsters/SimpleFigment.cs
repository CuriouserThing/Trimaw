using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.Models.Monsters;

/// <summary>
///     <see cref="OneShotFigment{TTrigger, TTalent}" /> with a parameterless intent.
/// </summary>
public abstract class SimpleFigment<TTrigger, TTalent, TIntent> : OneShotFigment<TTrigger, TTalent>
    where TTrigger : FigmentTrigger
    where TTalent : FigmentTalent
    where TIntent : FigmentIntent, new()
{
    protected sealed override TIntent GetFigmentIntent()
    {
        return new TIntent();
    }
}