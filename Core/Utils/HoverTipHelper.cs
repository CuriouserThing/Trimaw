using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Commands;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core.Utils;

public static class HoverTipHelper
{
    public static IEnumerable<IHoverTip> ForImagine(CardModel card)
    {
        var owner = card.IsCanonical ? null : card.Owner;
        var limit = owner is null
            ? ImaginationCmd.DefaultConcurrentFigmentLimit
            : MainFile.CombatManagerFactory.GetOrCreate(owner).ConcurrentFigmentLimit;
        return
        [
            HoverTipFactory.Static(StaticHoverTip.Imagine, new DynamicVar("Limit", limit)),
            HoverTipFactory.Static(StaticHoverTip.Pop)
        ];
    }

    public static IEnumerable<IHoverTip> ForMorselPrep(Morsel morsel)
    {
        var morselTip = morsel switch
        {
            Morsel.Bread => StaticHoverTip.Bread,
            Morsel.Meat => StaticHoverTip.Meat,
            Morsel.Shrooms => StaticHoverTip.Shrooms,
            Morsel.Berries => StaticHoverTip.Berries,
            Morsel.Pepper => StaticHoverTip.Pepper,
            Morsel.Water => StaticHoverTip.Water,
            _ => throw new ArgumentOutOfRangeException(nameof(morsel), morsel, null)
        };
        return [HoverTipFactory.Static(StaticHoverTip.Prep), HoverTipFactory.Static(morselTip)];
    }
}