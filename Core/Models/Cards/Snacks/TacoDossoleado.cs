using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Trimaw.Core.Models.Cards.Snacks;

public class TacoDossoleado : SnackCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        var energy = Owner.PlayerCombatState?.Energy ?? 1;
        var pool = Owner.Character.CardPool
            .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
            .Where(c => CardMatches(c, energy, IsUpgraded));
        var attack = CardFactory.GetDistinctForCombat(Owner, pool, 1, Owner.RunState.Rng.CombatCardGeneration)
            .FirstOrDefault();
        if (attack is null)
        {
            MainFile.Logger.Warn($"No attack with cost {energy} or X found in character's pool");
            return;
        }

        if (IsUpgraded) CardCmd.Upgrade(attack);
        await CardPileCmd.AddGeneratedCardToCombat(attack, PileType.Hand, Owner);
    }

    private bool CardMatches(CardModel card, int energy, bool upgraded)
    {
        if (upgraded)
        {
            card = card.ToMutable();
            card.UpgradeInternal();
        }

        return card.Type == CardType.Attack &&
               (card.EnergyCost.GetWithModifiers(CostModifiers.All) == energy || card.EnergyCost.CostsX);
    }
}