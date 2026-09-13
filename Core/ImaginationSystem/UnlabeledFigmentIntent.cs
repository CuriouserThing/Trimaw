using MegaCrit.Sts2.Core.Localization;
using Trimaw.Core.Models.Monsters;

namespace Trimaw.Core.ImaginationSystem;

public abstract class UnlabeledFigmentIntent<T> : FigmentIntent<T> where T : Figment
{
    private protected sealed override bool HasIntentLabel => false;

    protected sealed override void FormatIntentLabel(LocString label, MoveContext<T> ctx)
    {
    }
}