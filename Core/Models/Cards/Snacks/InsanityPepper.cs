using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Commands;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Powers.CeobePowers;

namespace Trimaw.Core.Models.Cards.Snacks;

public class InsanityPepper : SnackCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<AimPower>(3)];

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(AimPower)].UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ImaginationCmd.Imagine(choiceContext, Owner, FigmentFilter.All.Requiring(HardTag.Tiacauh));
        await PowerCmd.Apply<AimPower>(choiceContext, Owner.Creature, DynamicVars[nameof(AimPower)].IntValue,
            Owner.Creature, this);
    }
}