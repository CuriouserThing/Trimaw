using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.Models.Monsters;

/// <summary>
///     Basic <see cref="Figment" /> with <see cref="WispPower" /> and a single, parameterless intent.
/// </summary>
public abstract class WispFigment<TTrigger, TTalent, TIntent> : Figment<WispPower, TTrigger, TTalent>
    where TTrigger : FigmentTrigger
    where TTalent : FigmentTalent
    where TIntent : FigmentIntent, new()
{
    protected sealed override Task<FigmentIntent?> ReadyNextIntent(PlayerChoiceContext choiceContext)
    {
        return Task.FromResult<FigmentIntent?>(new TIntent());
    }
}