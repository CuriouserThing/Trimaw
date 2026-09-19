using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Trimaw.Core.Commands;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class FungimistPower : TrimawPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

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