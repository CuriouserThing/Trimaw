using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Triggers;

public class EotTrigger : FigmentMonoTrigger
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.NotoEmoji64("timer_clock");
    public override string Icon256Path => Pathfinder.NotoEmoji256("timer_clock");

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player) return;

        await TriggerMove(choiceContext);
    }
}