using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Commands;

namespace Trimaw.Core.Models.Cards.Basic;

public class Scrounge() : TrimawCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await SnackCmd.PrepPrevious(choiceContext, CombatState, Owner);
        await SnackCmd.PrepRandom(choiceContext, CombatState, Owner);
    }
}