using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Hooks;
using Trimaw.Core.Models.Cards;
using Trimaw.Core.SnackSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Triggers;

public class FieldRationTrigger : FigmentMonoTrigger, IPrepListener
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.NotoEmoji64("snail");
    public override string Icon256Path => Pathfinder.NotoEmoji256("snail");

    public async Task AfterMorselPrepped(PlayerChoiceContext choiceContext, Player player, Morsel morsel,
        SnackCard? createdSnack)
    {
        var morsels = MainFile.CombatManagerFactory.GetOrCreate(player).CurrentPrep;
        if (morsels.Count != 3) return;

        Figment.MarkForPopping();
        await TriggerMove(choiceContext);
        await Figment.Pop();
    }
}