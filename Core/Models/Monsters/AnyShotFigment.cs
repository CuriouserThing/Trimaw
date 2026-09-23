using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.Models.Monsters;

public abstract class AnyShotFigment<TTrigger, TTalent>(uint moveCount)
    : Figment<ImaginaryShieldPower, TTrigger, TTalent>
    where TTrigger : FigmentTrigger
    where TTalent : FigmentTalent
{
    protected sealed override void CountMovesRemaining(out uint min, out uint? max)
    {
        min = (uint)Math.Max(0, moveCount - MovesUsed);
        max = min;
    }

    protected sealed override FigmentIntent GetNextFigmentIntent()
    {
        return GetFigmentIntent(MovesUsed);
    }

    protected abstract FigmentIntent GetFigmentIntent(int idx);
}