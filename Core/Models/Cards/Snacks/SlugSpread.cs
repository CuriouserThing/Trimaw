using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Models.Enchantments;

namespace Trimaw.Core.Models.Cards.Snacks;

public class SlugSpread : SnackCard
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var reallyHeavy = ModelDb.Enchantment<ReallyHeavy>();
        var pool = Owner.Character.CardPool
            .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
            .Where(reallyHeavy.CanEnchant);
        var attack = CardFactory.GetDistinctForCombat(Owner, pool, 1, Owner.RunState.Rng.CombatCardGeneration)
            .FirstOrDefault();
        if (attack is null) return;
        CardCmd.Enchant<ReallyHeavy>(attack, 1);
        if (IsUpgraded) CardCmd.Upgrade(attack);
        await CardPileCmd.AddGeneratedCardToCombat(attack, PileType.Hand, Owner);
    }
}