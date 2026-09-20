using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Hooks;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class ObligateOmnivorePower : TrimawPower, IPrepListener
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public bool ShouldPreventMorselPrep(Player player, Morsel morsel)
    {
        return true;
    }

    public async Task AfterPreventingMorselPrep(PlayerChoiceContext choiceContext, Player player, Morsel morsel)
    {
        Flash();
        await PowerCmd.Decrement(this);
    }
}