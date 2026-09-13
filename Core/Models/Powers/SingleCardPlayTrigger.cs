using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Trimaw.Core.Models.Powers;

public abstract class SingleCardPlayTrigger : CardPlayTrigger
{
    public sealed override PowerStackType StackType => PowerStackType.Single;

    protected sealed override bool ShouldTriggerOnRemoval => false;

    public sealed override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.PetOwner || !CardMatches(cardPlay)) return;

        await TriggerMove(choiceContext);
    }
}