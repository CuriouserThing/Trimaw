using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers;

public class EvanescentPower : FigmentLifecyclePower
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.NotoEmoji64("wind_face");
    public override string Icon256Path => Pathfinder.NotoEmoji256("wind_face");

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Pop)];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (!Figment.IsAvailable || Owner.PetOwner != player) return;

        Figment.MarkForPopping();
        await Figment.UseMove(choiceContext, MoveParams.None, TriggerKind.Pop);
        await Figment.Pop();
    }
}