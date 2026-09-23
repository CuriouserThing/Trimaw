using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Trimaw.Core.Models.Cards.Common;

public class Roughhouse() : TrimawCard(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<VulnerablePower>(1),
        new CardsVar(2)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target,
            DynamicVars[nameof(VulnerablePower)].BaseValue, Owner.Creature, this);

        var count = Math.Min(
            DynamicVars.Cards.IntValue,
            CardPile.MaxCardsInHand - PileType.Hand.GetPile(Owner).Cards.Count);
        var cards = PileType.Discard.GetPile(Owner).Cards
            .Where(c => c.EnergyCost.GetWithModifiers(CostModifiers.All) == 1)
            .TakeRandom(count, Owner.RunState.Rng.CombatCardSelection)
            .ToArray();
        await CardPileCmd.Add(cards, PileType.Hand);
    }
}