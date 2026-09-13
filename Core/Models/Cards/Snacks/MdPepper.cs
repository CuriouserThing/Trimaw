using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Trimaw.Core.Models.Cards.Snacks;

public class MdPepper : SnackCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = DynamicVars.Cards.IntValue;
        for (var i = 0; i < cards; i += 1)
        {
            var card = (await CardPileCmd.Draw(choiceContext, 1, Owner)).FirstOrDefault();
            if (card is null) return;
            if (card.Enchantment is null)
                await PowerCmd.Apply<ClarityPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        }
    }
}