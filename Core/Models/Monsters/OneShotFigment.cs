using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.Models.Monsters;

/// <summary>
///     Basic <see cref="Figment" /> with <see cref="ImaginaryShieldPower" /> and a single intent.
/// </summary>
public abstract class OneShotFigment<TTrigger, TTalent> : Figment<ImaginaryShieldPower, TTrigger, TTalent>
    where TTrigger : FigmentTrigger
    where TTalent : FigmentTalent
{
    protected sealed override Task<FigmentIntent?> ReadyNextIntent(PlayerChoiceContext choiceContext)
    {
        return Task.FromResult<FigmentIntent?>(GetFigmentIntent());
    }

    protected abstract FigmentIntent GetFigmentIntent();
}