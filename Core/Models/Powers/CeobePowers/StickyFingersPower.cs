using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class StickyFingersPower : TrimawPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("dripping_honey");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("dripping_honey");

    public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources,
        CardLocation cardLocation)
    {
        ArgumentNullException.ThrowIfNull(Owner.Player);
        if (card.Owner.Creature != Owner || card.Enchantment is null) return cardLocation;

        return new CardLocation(Owner.Player, PileType.Hand, CardPilePosition.Bottom);
    }

    public override async Task AfterModifyingCardPlayResultLocation(CardModel card, CardLocation cardLocation)
    {
        Flash();
        await PowerCmd.Decrement(this);
    }
}