using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Commands;

namespace Trimaw.Core.Models.Cards.Common;

public class Sledgehammer() : TrimawCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    private const string AoeDamageKey = "AoeDamage";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6, ValueProp.Move),
        new DamageVar(AoeDamageKey, 5, ValueProp.Move)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars[AoeDamageKey].UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await TrimawCmd
            .Attack(this, cardPlay, DynamicVars.Damage.BaseValue)
            .Targeting(cardPlay.Target)
            .WithStaffAnimation()
            .Execute(choiceContext);

        ArgumentNullException.ThrowIfNull(CombatState);
        await TrimawCmd
            .Attack(this, cardPlay, DynamicVars[AoeDamageKey].BaseValue)
            .TargetingAllOpponents(CombatState)
            .Execute(choiceContext);
    }
}