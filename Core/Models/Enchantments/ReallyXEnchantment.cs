using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Trimaw.Core.Models.Enchantments;

public abstract class ReallyXEnchantment : TrimawEnchantment
{
    public override bool ShowAmount => true;

    protected abstract Task Operate();

    protected PlayerChoiceContext GetChoiceContext()
    {
        if (Card.CombatState?.CurrentSide == CombatSide.Enemy || !LocalContext.NetId.HasValue)
            return new BlockingPlayerChoiceContext();
        return new HookPlayerChoiceContext(Card.Owner, LocalContext.NetId.Value, GameActionType.Combat);
    }

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card != Card || card.Pile?.Type != PileType.Hand) return;

        await Operate();

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