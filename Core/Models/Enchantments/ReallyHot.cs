using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class ReallyHot : TrimawEnchantment
{
    public override bool ShowAmount => true;
    public override string Icon64Path => Pathfinder.NotoEmoji64("fire");

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card != Card || card.Pile?.Type != PileType.Hand) return;

        var creature = Card.Owner.Creature;
        _ = await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), creature, Amount, creature, null);

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