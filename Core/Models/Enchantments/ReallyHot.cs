using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class ReallyHot : ReallyXEnchantment
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];

    public override string Icon64Path => Pathfinder.NotoEmoji64("fire");

    protected override async Task Operate()
    {
        var choiceCtx = GetChoiceContext();
        var creature = Card.Owner.Creature;
        await PowerCmd.Apply<VigorPower>(choiceCtx, creature, Amount, creature, null);
    }
}