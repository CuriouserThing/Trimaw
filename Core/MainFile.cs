using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace Trimaw.Core;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "Trimaw";

    public static Logger Logger { get; } =
        new(ModId, LogType.Generic);

    public static ITrimawCombatManagerFactory CombatManagerFactory { get; } = new PineconeFactory();

    public static void Initialize()
    {
        Harmony harmony = new(ModId);
        harmony.PatchAll();
    }
}