using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core.Utils;

public static class HoverTipHelper
{
    public static IEnumerable<IHoverTip> ForManifest<T>() where T : EnchantmentModel
    {
        return new[] { HoverTipFactory.Static(StaticHoverTip.Manifest) }.Concat(HoverTipFactory.FromEnchantment<T>());
    }

    public static IEnumerable<IHoverTip> ForImagineRandom<T>(CardModel card) where T : FigmentPower
    {
        return ForImagine(card).Concat([HoverTipFactory.FromPower<T>()]);
    }

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

    public static IEnumerable<IHoverTip> ForMorselPrep(params Morsel[] morsels)
    {
        var tips = new IHoverTip[morsels.Length + 1];
        tips[0] = HoverTipFactory.Static(StaticHoverTip.Prep);
        for (var i = 0; i < morsels.Length; i += 1)
        {
            var morselTip = morsels[i] switch
            {
                Morsel.Bread => StaticHoverTip.Bread,
                Morsel.Meat => StaticHoverTip.Meat,
                Morsel.Shrooms => StaticHoverTip.Shrooms,
                Morsel.Berries => StaticHoverTip.Berries,
                Morsel.Pepper => StaticHoverTip.Pepper,
                Morsel.Water => StaticHoverTip.Water,
                _ => throw new ArgumentOutOfRangeException(nameof(morsels), morsels, null)
            };
            tips[i + 1] = HoverTipFactory.Static(morselTip);
        }

        return tips;
    }
}