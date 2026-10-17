using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Commands;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Relics;

public class Lunchbox : TrimawRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override string Icon85Path => Pathfinder.NotoEmoji85("bento_box");
    public override string Icon256Path => Pathfinder.NotoEmoji256("bento_box");

    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<Cooler>();
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
        IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature) ||
            Owner.PlayerCombatState is not { } playerCombatState ||
            playerCombatState.TurnNumber > 1) return;

        await SnackCmd.PrepRandom(choiceContext, combatState, Owner);
        await SnackCmd.PrepRandom(choiceContext, combatState, Owner);
    }
}