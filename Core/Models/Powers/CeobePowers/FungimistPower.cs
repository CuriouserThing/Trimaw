using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Trimaw.Core.Commands;
using Trimaw.Core.SnackSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class FungimistPower : TrimawPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("fragrance");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("fragrance");

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Prep),
        HoverTipFactory.Static(StaticHoverTip.Shrooms)
    ];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;

        Flash();
        for (var i = 0; i < Amount; i += 1)
            await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Shrooms, player);
    }
}