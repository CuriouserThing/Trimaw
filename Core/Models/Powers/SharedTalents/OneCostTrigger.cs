using MegaCrit.Sts2.Core.Entities.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.SharedTalents;

public class OneCostTrigger : MultiCardPlayTrigger
{
    public override string Icon64Path => Pathfinder.GameIconsDotnet64("armor_blueprint");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("armor_blueprint");

    protected override bool CardMatches(CardPlay cardPlay)
    {
        return cardPlay.Resources.EnergyValue == 1;
    }
}