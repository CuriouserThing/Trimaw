using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Commands;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Relics;

public class Cooler : TrimawRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override string Icon85Path => Pathfinder.GameIconsDotnet85("cooler");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("cooler");

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
        IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature) || Owner.PlayerCombatState is not { } playerCombatState) return;

        if (playerCombatState.TurnNumber > 1) await SnackCmd.PrepRandom(choiceContext, combatState, Owner);
        await SnackCmd.PrepRandom(choiceContext, combatState, Owner);
    }
}