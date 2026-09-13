using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.SharedTalents;

public class CardsDrawnTrigger : SingleTriggerTalent
{
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override bool ShouldTriggerOnRemoval => true;

    public override string Icon64Path => Pathfinder.NotoEmoji64("closed_book");
    public override string Icon256Path => Pathfinder.NotoEmoji256("closed_book");

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner != Figment.Creature.PetOwner || fromHandDraw) return;

        await PowerCmd.Decrement(this);
    }
}