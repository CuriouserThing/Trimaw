using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.SharedTalents;

public class CardsAddedTrigger : SingleTriggerTalent
{
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override bool ShouldTriggerOnRemoval => true;

    public override string Icon64Path => Pathfinder.NotoEmoji64("pencil");
    public override string Icon256Path => Pathfinder.NotoEmoji256("pencil");

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card.Owner != Figment.Creature.PetOwner ||
            card.Pile?.Type != PileType.Hand)
            return;

        await PowerCmd.Decrement(this);
    }
}