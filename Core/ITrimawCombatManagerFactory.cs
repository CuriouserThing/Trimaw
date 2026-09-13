using MegaCrit.Sts2.Core.Entities.Players;

namespace Trimaw.Core;

public interface ITrimawCombatManagerFactory
{
    ITrimawCombatManager Register(Player player, PlayerCombatState combatState);

    ITrimawCombatManager GetOrCreate(Player player);
}