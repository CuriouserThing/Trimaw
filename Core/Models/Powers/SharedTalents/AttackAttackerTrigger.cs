using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.SharedTalents;

public class AttackAttackerTrigger : SingleTriggerTalent
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.NotoEmoji64("popcorn");
    public override string Icon256Path => Pathfinder.NotoEmoji256("popcorn");

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.PetOwner ||
            cardPlay.Target?.Monster?.IntendsToAttack is not true)
            return;

        await TriggerMove(choiceContext);
    }
}