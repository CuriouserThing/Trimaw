using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Models.Cards.Tokens;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class DartboardPower : TrimawPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Dart>()];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;

        Flash();
        var cards = new CardModel[Amount];
        for (var i = 0; i < Amount; i += 1) cards[i] = CombatState.CreateCard<Dart>(player);
        await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, player);
    }
}