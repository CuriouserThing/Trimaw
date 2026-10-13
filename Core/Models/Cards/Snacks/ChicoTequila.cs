using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using Trimaw.Core.Commands;
using Trimaw.Core.Utils;

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
        var enchanter = new Enchanter<Swift>(DynamicVars[nameof(Swift)].IntValue);
        var transformed = await EnchantCmd.GenerateCards(choiceContext, Owner, enchanter, original.Length);
        var transforms = original.Zip(transformed, (a, b) => new CardTransformation(a, b));
        await CardCmd.Transform(transforms, null);
    }
}