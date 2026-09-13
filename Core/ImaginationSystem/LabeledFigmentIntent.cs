using Trimaw.Core.Models.Monsters;

namespace Trimaw.Core.ImaginationSystem;

public abstract class LabeledFigmentIntent<T> : FigmentIntent<T> where T : Figment
{
    private protected sealed override bool HasIntentLabel => true;
}