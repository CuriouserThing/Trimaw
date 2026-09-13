using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Commands;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core.Models.Cards.Snacks;

public class BoneAppleTea : SnackCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);

        int attackCount = 0, nonAttackCount = 0;
        foreach (var card in cards)
            if (card.Type == CardType.Attack) attackCount += 1;
            else nonAttackCount += 1;

        for (var i = 0; i < attackCount; i += 1)
            await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Shrooms, Owner);

        for (var i = 0; i < nonAttackCount; i += 1)
            await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Bread, Owner);
    }
}