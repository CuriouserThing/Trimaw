using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Enchantments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class BadFurDay() : TrimawCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipHelper.ForManifest<ReallyDry>();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(4),
        new(nameof(ReallyDry), 3)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars[nameof(ReallyDry)].UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var enchanter = new Enchanter<ReallyDry>(DynamicVars[nameof(ReallyDry)].IntValue);
        var cardCount = DynamicVars.Cards.IntValue;
        await EnchantCmd.Manifest(choiceContext, Owner, PileType.Hand, enchanter, cardCount);
    }
}