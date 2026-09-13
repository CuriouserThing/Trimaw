using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Trimaw.Core.Models.Cards.Tokens;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class Skewered : TrimawEnchantment
{
    public override bool HasExtraCardText => true;
    public override string Icon64Path => Pathfinder.NotoEmoji64("oden");

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Dart>()];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card != Card || Card.CombatState is not { } combatState) return;

        var card = combatState.CreateCard<Dart>(Card.Owner);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Card.Owner);
    }
}