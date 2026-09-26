using MegaCrit.Sts2.Core.Entities.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Triggers;

public class BearNecessitiesTrigger : MultiCardPlayTrigger
{
    public override string Icon64Path => Pathfinder.GameIconsDotnet64("tree_beehive");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("tree_beehive");

    protected override bool CardMatches(CardPlay cardPlay)
    {
        return cardPlay.Resources.EnergySpent >= 1;
    }
}