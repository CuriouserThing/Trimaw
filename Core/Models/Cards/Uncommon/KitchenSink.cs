using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Enchantments;
using Trimaw.Core.SnackSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class KitchenSink() : TrimawCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.Static(StaticHoverTip.Manifest) }
            .Concat(HoverTipFactory.FromEnchantment<ReallyHot>())
            .Concat(HoverTipFactory.FromEnchantment<ReallyCold>())
            .Concat(HoverTipHelper.ForMorselPrep(Morsel.Water));

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(nameof(ReallyHot), 4),
        new(nameof(ReallyCold), 4)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(ReallyHot)].UpgradeValueBy(2);
        DynamicVars[nameof(ReallyCold)].UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Enchanter[] enchanters =
        [
            new Enchanter<ReallyHot>(DynamicVars[nameof(ReallyHot)].IntValue),
            new Enchanter<ReallyCold>(DynamicVars[nameof(ReallyCold)].IntValue)
        ];
        await EnchantCmd.Manifest(choiceContext, Owner, PileType.Draw, enchanters);

        await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Water, Owner);
    }
}