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
        var polished = ModelDb.Enchantment<Polished>();
        var hand = PileType.Hand.GetPile(Owner).Cards.Where(polished.CanEnchant);
        if (IsUpgraded)
            hand = hand
                .GroupBy(c => c.EnergyCost.GetWithModifiers(CostModifiers.Local))
                .OrderByDescending(g => g.Key)
                .FirstOrDefault();
        var card = hand?.TakeRandom(1, Owner.RunState.Rng.CombatCardSelection).FirstOrDefault();
        if (card is null) return;

        var clone = card.CreateClone();
        CardCmd.ClearEnchantment(clone);
        CardCmd.Enchant<Polished>(clone, 1);
        await CardCmd.Transform(card, clone);
    }
}