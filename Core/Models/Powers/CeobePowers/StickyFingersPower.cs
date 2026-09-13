using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Trimaw.Core.Commands;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class StickyFingersPower : TrimawPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("dripping_honey");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("dripping_honey");

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.SuperSpecial)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay.Card;
        if (card.Owner.Creature != Owner) return;

        var clone = card.CreateClone();
        CardCmd.ClearEnchantment(clone);
        await TrimawCmd.MakeSuperSpecial(clone);
        await CardPileCmd.AddGeneratedCardToCombat(clone, PileType.Hand, card.Owner);
        await PowerCmd.Decrement(this);
    }
}