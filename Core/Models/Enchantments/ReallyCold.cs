using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Trimaw.Core.Models.Enchantments;

public class ReallyCold : TrimawEnchantment
{
    public override bool ShowAmount => true;

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card != Card || card.Pile?.Type != PileType.Hand) return;

        var creature = Card.Owner.Creature;
        await CreatureCmd.GainBlock(creature, Amount, ValueProp.Unpowered, null);

        if (Amount > 1)
        {
            Amount -= 1;
        }
        else
        {
            Amount = 0;
            Status = EnchantmentStatus.Disabled;
        }
    }
}