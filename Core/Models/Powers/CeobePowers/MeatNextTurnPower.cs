using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Trimaw.Core.Commands;
using Trimaw.Core.SnackSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class MeatNextTurnPower : TrimawPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("camp_cooking_pot");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("camp_cooking_pot");

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipHelper.ForMorselPrep(Morsel.Meat);

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;

        Flash();
        for (var i = 0; i < Amount; i += 1)
            await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Meat, Owner.Player);
        await PowerCmd.Remove(this);
    }
}