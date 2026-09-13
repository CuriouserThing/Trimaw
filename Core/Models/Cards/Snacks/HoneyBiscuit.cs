using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Models.Enchantments;

namespace Trimaw.Core.Models.Cards.Snacks;

public class HoneyBiscuit : SnackCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Polished>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<CardModel> pileCards = PileType.Discard.GetPile(Owner).Cards;
        if (IsUpgraded) pileCards = pileCards.Concat(PileType.Draw.GetPile(Owner).Cards);

        var polished = ModelDb.Enchantment<Polished>();
        var candidates = pileCards
            .Where(polished.CanEnchant)
            .GroupBy(c => c.EnergyCost.GetWithModifiers(CostModifiers.Local))
            .OrderByDescending(g => g.Key)
            .FirstOrDefault();
        var card = candidates?.TakeRandom(1, Owner.RunState.Rng.CombatCardSelection).FirstOrDefault();
        if (card is null) return;

        var clone = card.CreateClone();
        CardCmd.ClearEnchantment(clone);
        CardCmd.Enchant<Polished>(clone, 1);
        await CardCmd.Transform(card, clone);
    }
}