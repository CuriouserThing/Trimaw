using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Commands;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class MiseEnPlace() : TrimawCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var morsels = MainFile.CombatManagerFactory.GetOrCreate(Owner).CurrentPrep.Count;
        await CardPileCmd.Draw(choiceContext, morsels, Owner);
        SnackCard? snack = null;
        while (snack is null)
        {
            snack = await SnackCmd.PrepRandom(choiceContext, CombatState, Owner);
        }
    }
}