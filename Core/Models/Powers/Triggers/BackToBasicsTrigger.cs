using MegaCrit.Sts2.Core.Entities.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Triggers;

public class BackToBasicsTrigger : MultiCardPlayTrigger
{
    public override string Icon64Path => Pathfinder.GameIconsDotnet64("card_play");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("card_play");

    protected override bool CardMatches(CardPlay cardPlay)
    {
        return cardPlay.Card.Tags.Any(t => t is CardTag.Strike or CardTag.Defend);
    }
}