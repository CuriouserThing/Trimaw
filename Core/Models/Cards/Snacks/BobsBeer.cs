using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace Trimaw.Core.Models.Cards.Snacks;

public class BobsBeer : SnackCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(2),
        new EnergyVar(3)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);

        foreach (var card in PileType.Hand.GetPile(Owner).Cards)
        {
            if (card.EnergyCost.GetWithModifiers(CostModifiers.None) < 0 ||
                card.EnergyCost.CostsX)
                continue;

            var cost = Owner.RunState.Rng.CombatEnergyCosts.NextInt(DynamicVars.Energy.IntValue + 1);
            card.EnergyCost.SetThisTurnOrUntilPlayed(cost);
            NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
        }
    }
}