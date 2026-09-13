using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class Polished : TrimawEnchantment
{
    public override bool HasExtraCardText => true;
    public override string Icon64Path => Pathfinder.GameIconsDotnet64("sparkling_sabre");

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override bool CanEnchant(CardModel card)
    {
        return card.EnergyCost.Canonical > 0;
    }

    protected override void OnEnchant()
    {
        Card.EnergyCost.SetThisCombat(0);
        Card.SetStarCostThisCombat(0); // sure why not, free is free
    }

    public override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        cardPlay?.Card.EnergyCost.AddThisCombat(1);
        return Task.CompletedTask;
    }
}