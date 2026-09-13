using MegaCrit.Sts2.Core.Entities.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.SharedTalents;

public class SkillTrigger : MultiCardPlayTrigger
{
    public override string Icon64Path => Pathfinder.GameIconsDotnet64("confirmed");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("confirmed");

    protected override bool CardMatches(CardPlay cardPlay)
    {
        return cardPlay.Card.Type == CardType.Skill;
    }
}