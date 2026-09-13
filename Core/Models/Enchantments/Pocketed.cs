using MegaCrit.Sts2.Core.Entities.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class Pocketed : TrimawEnchantment
{
    public override string Icon64Path => Pathfinder.NotoEmoji64("flatbread");

    protected override void OnEnchant()
    {
        Card.AddKeyword(CardKeyword.Retain);
    }
}