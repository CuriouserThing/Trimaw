using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Commands;

namespace Trimaw.Core.Models.Cards.Snacks;

public class ChicoTequila : SnackCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.SuperSpecial)];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);

        var handAttacks = PileType.Hand.GetPile(Owner).Cards
            .Where(c => c.Type == CardType.Attack)
            .ToArray();

        var transformedAttacks = CardFactory.GetDistinctForCombat(
            Owner,
            Owner.Character.CardPool
                .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
                .Where(c => c.Type == CardType.Attack),
            handAttacks.Length,
            Owner.RunState.Rng.CombatCardGeneration).ToArray();

        foreach (var attack in transformedAttacks) await TrimawCmd.MakeSuperSpecial(attack);

        var transforms = handAttacks.Zip(transformedAttacks, (a, b) => new CardTransformation(a, b));
        await CardCmd.Transform(transforms, null);
    }
}