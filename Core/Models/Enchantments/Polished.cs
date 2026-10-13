using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class Polished : TrimawEnchantment
{
    private int _energyIncreaseRemaining;

    public override bool HasExtraCardText => true;
    public override string Icon64Path => Pathfinder.GameIconsDotnet64("sparkling_sabre");

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    private static int GetCost(CardModel card)
    {
        return card.EnergyCost.GetWithModifiers(CostModifiers.None);
    }

    public override bool CanEnchant(CardModel card)
    {
        return base.CanEnchant(card) && GetCost(card) > 0;
    }

    protected override void OnEnchant()
    {
        if (!IsMutable) return;

        _energyIncreaseRemaining = GetCost(Card);
        Card.EnergyCost.SetThisCombat(0);
        Card.SetStarCostThisCombat(0); // sure why not, free is free
    }

    public override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        if (!IsMutable) return Task.CompletedTask;

        if (_energyIncreaseRemaining > 0)
        {
            cardPlay?.Card.EnergyCost.AddThisCombat(1);
            _energyIncreaseRemaining -= 1;
            if (_energyIncreaseRemaining == 0) Status = EnchantmentStatus.Disabled;
        }

        return Task.CompletedTask;
    }
}