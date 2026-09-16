using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Commands;

namespace Trimaw.Core.Models.Cards.Rare;

public class DwcTrophy() : TrimawCard(4, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(24),
        new ExtraDamageVar(2),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(GetMultiplier)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(6);
        DynamicVars.ExtraDamage.UpgradeValueBy(2);
    }

    private static decimal GetMultiplier(CardModel card, Creature? creature)
    {
        return CombatManager.Instance.History.Entries
            .OfType<CardPlayFinishedEntry>()
            .Count(c => c.CardPlay.Resources.EnergySpent > 0);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await TrimawCmd
            .Attack(this, cardPlay, DynamicVars.CalculatedDamage.Calculate(cardPlay.Target))
            .Targeting(cardPlay.Target)
            .WithSpearAnimation()
            .WithTimescale(0.7f)
            .Execute(choiceContext);
    }
}