using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Trimaw.Core.Models.Powers.CeobePowers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class Glazed : TrimawEnchantment
{
    public override bool HasExtraCardText => true;
    public override string Icon64Path => Pathfinder.GameIconsDotnet64("dripping_honey");

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<StickyFingersPower>(),
        HoverTipFactory.Static(StaticHoverTip.SuperSpecial)
    ];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card != Card) return;

        var creature = Card.Owner.Creature;
        _ = await PowerCmd.Apply<StickyFingersPower>(choiceContext, creature, 1, creature, Card);
    }
}