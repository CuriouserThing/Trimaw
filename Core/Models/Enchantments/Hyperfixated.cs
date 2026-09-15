using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Trimaw.Core.Models.Enchantments;

public class Hyperfixated : TrimawEnchantment
{
    public override bool HasExtraCardText => true;

    public override bool CanEnchant(CardModel card)
    {
        return base.CanEnchant(card) &&
               !card.EnergyCost.CostsX &&
               card.CanonicalStarCost < 1 &&
               card.EnergyCost.GetWithModifiers(CostModifiers.None) < 2;
    }

    protected override void OnEnchant()
    {
        Card.SetToFreeThisCombat();
    }

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card != Card) return playCount;

        // Energy spent is only >0 with some cost modifier in effect, but we want the card to behave as an X-cost,
        // so we have to estimate the amount of energy spent on it.
        // Narrow corner case so if the estimate is wrong because of some timing issue it's not the end of the world :)
        var energySpentEstimate = card.EnergyCost.GetWithModifiers(CostModifiers.All);
        
        // Hardcode this special case :)
        var relicBonus = card.Owner.Relics.Count(r => r is ChemicalX) * 2;
        
        var remainingEnergy = card.Owner.PlayerCombatState?.Energy ?? 0;
        return playCount + energySpentEstimate + relicBonus + remainingEnergy;
    }

    public override async Task AfterModifyingCardPlayCount(CardModel card)
    {
        await PlayerCmd.SetEnergy(0, card.Owner);
    }
}