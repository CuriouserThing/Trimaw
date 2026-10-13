using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Enchantments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class DailyDoodlesPower : TrimawPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string Icon64Path => Pathfinder.NotoEmoji64("frame_with_picture");
    public override string Icon256Path => Pathfinder.NotoEmoji256("frame_with_picture");

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipHelper.ForManifest<Doodled>();

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;

        await EnchantCmd.Manifest(choiceContext, player, PileType.Hand, new Enchanter<Doodled>(), Amount);
    }
}