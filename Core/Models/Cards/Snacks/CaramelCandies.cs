using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace Trimaw.Core.Models.Cards.Snacks;

public class CaramelCandies : SnackCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var glam = ModelDb.Enchantment<Glam>();
        IEnumerable<CardModel> source = PileType.Hand.GetPile(Owner).Cards;
        if (IsUpgraded) source = source.Concat(PileType.Draw.GetPile(Owner).Cards);
        var cards = source
            .Where(c => glam.CanEnchant(c) &&
                        c.EnergyCost.GetWithModifiers(CostModifiers.All) == DynamicVars.Energy.IntValue)
            .ToArray();
        if (cards.Length < 1) return;

        var glammedCards = cards.Select(c => c.CreateClone()).ToArray();
        foreach (var card in glammedCards)
        {
            CardCmd.ClearEnchantment(card);
            CardCmd.Enchant<Glam>(card, 1);
        }

        var transforms = cards.Zip(glammedCards, (a, b) => new CardTransformation(a, b));
        await CardCmd.Transform(transforms, null);
    }
}