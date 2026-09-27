using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Triggers;

public class FetchTrigger : FigmentMonoTrigger
{
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override bool ShouldTriggerOnRemoval => true;

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("dog_bowl");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("dog_bowl");

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card.Owner != Figment.Creature.PetOwner ||
            card.Pile?.Type != PileType.Hand)
            return;

        await PowerCmd.Decrement(this);
    }
}