using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Enchantments;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class BadFurDay() : TrimawCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
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
        var hand = CardPile.Get(PileType.Hand, Owner)?.Cards ?? [];
        var draw = CardPile.Get(PileType.Draw, Owner)?.Cards ?? [];
        var cardCount = DynamicVars.Cards.IntValue;
        var enchantAmount = DynamicVars[nameof(ReallyDry)].IntValue;
        await TrimawCmd.TransformIntoEnchantedCopies<ReallyDry>(Owner, hand.Concat(draw), cardCount, enchantAmount);
    }
}