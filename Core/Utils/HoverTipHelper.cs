using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Commands;

namespace Trimaw.Core.Utils;

public static class HoverTipHelper
{
    public static IHoverTip ForImagine(CardModel card)
    {
        Player? owner = card.IsCanonical ? null : card.Owner;
        var limit = owner is null
            ? ImaginationCmd.DefaultConcurrentFigmentLimit
            : MainFile.CombatManagerFactory.GetOrCreate(owner).ConcurrentFigmentLimit;
        return HoverTipFactory.Static(StaticHoverTip.Imagine, new DynamicVar("Limit", limit));
    }
}