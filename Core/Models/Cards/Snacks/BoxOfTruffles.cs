using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Commands;

namespace Trimaw.Core.Models.Cards.Snacks;

public class BoxOfTruffles : SnackCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(1)];

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var times = 1 + DynamicVars.Repeat.IntValue;
        for (var i = 0; i < times; i += 1)
            if (Owner.RunState.Rng.CombatCardSelection.NextBool())
            {
                await ImaginationCmd.ImagineGachaPull(choiceContext, Owner);
            }
            else
            {
                var pool = Owner.Character.CardPool
                    .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
                    .Where(c => c.Type == CardType.Skill);
                var skill = CardFactory.GetForCombat(Owner, pool, 1, Owner.RunState.Rng.CombatCardGeneration)
                    .FirstOrDefault();
                if (skill is null) continue;
                await TrimawCmd.MakeSuperSpecial(skill);
                await CardPileCmd.AddGeneratedCardToCombat(skill, PileType.Hand, Owner);
            }
    }
}