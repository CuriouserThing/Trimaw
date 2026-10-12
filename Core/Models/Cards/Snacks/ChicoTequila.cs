using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace Trimaw.Core.Models.Cards.Snacks;

public class ChicoTequila : SnackCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Swift>();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new(nameof(Swift), 1)];

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(Swift)].UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var original = PileType.Hand.GetPile(Owner).Cards.ToArray();
        var transformed = CardFactory.GetDistinctForCombat(
            Owner,
            Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint),
            original.Length,
            Owner.RunState.Rng.CombatCardGeneration).ToArray();
        foreach (var card in transformed) CardCmd.Enchant<Swift>(card, DynamicVars[nameof(Swift)].BaseValue);
        var transforms = original.Zip(transformed, (a, b) => new CardTransformation(a, b));
        await CardCmd.Transform(transforms, null);
    }
}