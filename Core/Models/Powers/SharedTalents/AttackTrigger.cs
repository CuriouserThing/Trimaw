using MegaCrit.Sts2.Core.Entities.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.SharedTalents;

public class AttackTrigger : MultiCardPlayTrigger
{
    public override string Icon64Path => Pathfinder.NotoEmoji64("pager");
    public override string Icon256Path => Pathfinder.NotoEmoji256("pager");

    protected override bool CardMatches(CardPlay cardPlay)
    {
        return cardPlay.Card.Type == CardType.Attack;
    }
}