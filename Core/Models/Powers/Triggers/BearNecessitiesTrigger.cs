using MegaCrit.Sts2.Core.Entities.Cards;

namespace Trimaw.Core.Models.Powers.Triggers;

public class BearNecessitiesTrigger : MultiCardPlayTrigger
{
    protected override bool CardMatches(CardPlay cardPlay)
    {
        return cardPlay.Resources.EnergySpent >= 1;
    }
}