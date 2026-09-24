using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class ReallyCold : ReallyXEnchantment
{
    public override string Icon64Path => Pathfinder.NotoEmoji64("ice_cube");

    protected override async Task Operate()
    {
        await CreatureCmd.GainBlock(Card.Owner.Creature, Amount, ValueProp.Unpowered, null);
    }
}