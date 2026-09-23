using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Triggers;

public class BigUglyThingTrigger : FigmentPolyTrigger
{
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new HpLossVar(7)];

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("overdrive");

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Player != Owner.PetOwner || cardPlay.Resources.EnergySpent < 1) return;

        await TriggerMove(choiceContext);
        if (Figment is BigUglyThingFigment bigUgly && bigUgly.IsInMechPhase())
        {
            await CreatureCmd.LoseMaxHp(choiceContext, Owner, DynamicVars.HpLoss.BaseValue, false);
            await PowerCmd.Decrement(this);
        }
    }
}