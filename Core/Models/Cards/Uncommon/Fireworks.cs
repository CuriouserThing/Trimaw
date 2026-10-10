using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Commands;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class Fireworks() : TrimawCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(0),
        new CalculationExtraVar(2),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(CalculateDamage)
    ];

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }

    private static decimal CalculateDamage(CardModel card, Creature? creature)
    {
        return card.Owner.PlayerCombatState?.AllCards.Count(c => c.Enchantment is not null) ?? 0;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);
        await TrimawCmd
            .Attack(this, cardPlay, DynamicVars.CalculatedDamage.Calculate(null))
            .TargetingAllOpponents(CombatState)
            .WithStaffAnimation()
            .Execute(choiceContext);
    }
}