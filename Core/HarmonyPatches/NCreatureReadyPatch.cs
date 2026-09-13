using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using Trimaw.Core.GodotControls;
using Trimaw.Core.Utils;

namespace Trimaw.Core.HarmonyPatches;

[HarmonyPatch(typeof(NCreature), nameof(NCreature._Ready))]
internal class NCreatureReadyPatch
{
    [HarmonyPrefix]
    private static void Prefix(NCreature __instance)
    {
        if (!__instance.Entity.IsPlayer) return;

        var scene = GD.Load<PackedScene>(Pathfinder.Scene("morsel_stock"));
        var stock = scene.Instantiate<Control>();
        __instance.AddChildSafely(stock);
        MorselStockWrapper.NCreatureTable[__instance] = new MorselStockWrapper(stock);
    }
}