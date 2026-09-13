using MegaCrit.Sts2.Core.Entities.Cards;

namespace Trimaw.Core.Models.Powers;

public abstract class CardPlayTrigger : SingleTriggerTalent
{
    protected abstract bool CardMatches(CardPlay cardPlay);
}