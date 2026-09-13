using MegaCrit.Sts2.Core.Entities.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.SharedTalents;

public class AnyEnergySpentTrigger : SingleCardPlayTrigger
{
    public override string Icon64Path => Pathfinder.GameIconsDotnet64("arrowhead");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("arrowhead");

    protected override bool CardMatches(CardPlay cardPlay)
    {
        return cardPlay.Resources.EnergySpent >= 1;
    }
}