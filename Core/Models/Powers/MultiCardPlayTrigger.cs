using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Trimaw.Core.Models.Powers;

public abstract class MultiCardPlayTrigger : CardPlayTrigger
{
    public sealed override PowerStackType StackType => PowerStackType.Counter;

    protected sealed override bool ShouldTriggerOnRemoval => true;

    public sealed override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CardTriggersMove(cardPlay)) await PowerCmd.Decrement(this);
    }
}