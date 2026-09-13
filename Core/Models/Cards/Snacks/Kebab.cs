using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Monsters.Figments;

namespace Trimaw.Core.Models.Cards.Snacks;

public class Kebab : SnackCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ImaginationCmd.Imagine<VulcanFigment>(choiceContext, Owner);

        foreach (var card in PileType.Discard.GetPile(Owner).Cards
                     .Where(c => c.Type == CardType.Attack)
                     .TakeRandom(DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardSelection)
                     .ToArray())
            await CardPileCmd.Add(card, PileType.Hand);
    }
}