using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Triggers;

public class FollowTheLeaderTrigger : SingleCardPlayTrigger
{
    public override string Icon64Path => Pathfinder.NotoEmoji64("drum_with_drumsticks");
    public override string Icon256Path => Pathfinder.NotoEmoji256("drum_with_drumsticks");

    protected override bool CardMatches(CardPlay cardPlay)
    {
        // Don't count X-costs unless the wording more intuitively reflects that they're also triggers
        return cardPlay.Resources.EnergyValue == 1 &&
               !cardPlay.Card.EnergyCost.CostsX &&
               cardPlay.Target is { Side: CombatSide.Enemy };
    }

    protected override MoveParams CreateMoveParams(CardPlay cardPlay)
    {
        return new MoveParams { EnemyTarget = cardPlay.Target };
    }
}