using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Talents;

public class AlleyOopTalent : FigmentTalent
{
    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<UltimateStrike>()];

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("ricochet");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("ricochet");

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Figment.PetOwner) return;

        Flash();
        var card = CombatState.CreateCard<UltimateStrike>(player);
        CardCmd.ApplyKeyword(card, CardKeyword.Exhaust);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, player);
        await PowerCmd.Remove(this);
    }
}