using System.Diagnostics.CodeAnalysis;
using BaseLib.Patches.Content;
using MegaCritTip = MegaCrit.Sts2.Core.HoverTips.StaticHoverTip;

namespace Trimaw.Core;

// ReSharper disable UnassignedField.Global
[SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible")]
public static class StaticHoverTip
{
    [CustomEnum] public static MegaCritTip SuperSpecial;

    [CustomEnum] public static MegaCritTip Prep;
    [CustomEnum] public static MegaCritTip Bread;
    [CustomEnum] public static MegaCritTip Meat;
    [CustomEnum] public static MegaCritTip Shrooms;
    [CustomEnum] public static MegaCritTip Berries;
    [CustomEnum] public static MegaCritTip Pepper;
    [CustomEnum] public static MegaCritTip Water;

    [CustomEnum] public static MegaCritTip Imagine;
    [CustomEnum] public static MegaCritTip Pop;
    [CustomEnum] public static MegaCritTip DuckLordAssociate;
    [CustomEnum] public static MegaCritTip Eunectes;
}