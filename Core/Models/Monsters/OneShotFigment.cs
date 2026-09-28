using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.Models.Monsters;

/// <summary>
///     Basic <see cref="Figment" /> with <see cref="ImaginaryFriendPower" /> and a single intent.
/// </summary>
public abstract class OneShotFigment<TTrigger, TTalent> : Figment<ImaginaryFriendPower, TTrigger, TTalent>
    where TTrigger : FigmentTrigger
    where TTalent : FigmentTalent
{
    protected sealed override Task<FigmentIntent?> ReadyNextIntent(PlayerChoiceContext choiceContext)
    {
        return MovesUsed switch
        {
            0 => Task.FromResult<FigmentIntent?>(GetFigmentIntent()),
            _ => Task.FromResult<FigmentIntent?>(null)
        };
    }

    protected abstract FigmentIntent GetFigmentIntent();
}