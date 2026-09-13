using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Trimaw.Core.Models.Cards.Snacks;

public class UrsusBigBread : SnackCard
{
    private const string EnergyLossKey = "EnergyLoss";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(EnergyLossKey, 1),
        new EnergyVar(4)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.LoseEnergy(DynamicVars[EnergyLossKey].BaseValue, Owner);
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Energy.BaseValue,
            Owner.Creature, this);
    }
}