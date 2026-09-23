using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Triggers;

public class TargetTrigger : FigmentMonoTrigger
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.NotoEmoji64("drum_with_drumsticks");
    public override string Icon256Path => Pathfinder.NotoEmoji256("drum_with_drumsticks");

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Player != Owner.PetOwner ||
            cardPlay.Target is not { } target) return;

        await TriggerMove(choiceContext, new MoveParams { Target = target });
    }
}