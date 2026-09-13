using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Trimaw.Core.HarmonyPatches;

[HarmonyPatch(typeof(PlayerCombatState), MethodType.Constructor)]
[HarmonyPatch([typeof(Player)])]
internal class PlayerCombatStateConstructorPatch
{
    [HarmonyPostfix]
    private static void Postfix(Player player, PlayerCombatState __instance)
    {
        MainFile.CombatManagerFactory.Register(player, __instance);
    }
}