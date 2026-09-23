using MegaCrit.Sts2.Core.Entities.Cards;

namespace Trimaw.Core.Models.Powers.Triggers;

public class BackToBasicsTrigger : MultiCardPlayTrigger
{
    protected override bool CardMatches(CardPlay cardPlay)
    {
        return cardPlay.Card.Tags.Any(t => t is CardTag.Strike or CardTag.Defend);
    }
}