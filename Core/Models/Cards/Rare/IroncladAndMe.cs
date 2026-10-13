using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Enchantments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Rare;

public class IroncladAndMe() : TrimawCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Doodled>();

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var enchanter = new Enchanter<Doodled> { CanPotentiallyEnchantPredicate = CanPotentiallyEnchant };
        CardModel[] cards =
        [
            await EnchantCmd.GenerateCard(choiceContext, Owner, enchanter),
            await EnchantCmd.GenerateCard(choiceContext, Owner, enchanter, ModelDb.Character<Ironclad>().CardPool)
        ];
        if (IsUpgraded)
            foreach (var card in cards)
                CardCmd.Upgrade(card);
        await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, Owner);
    }

    private static bool CanPotentiallyEnchant(CardModel card)
    {
        // Design of this could go a few ways. We disallow cards that can't be Doodled after upgrade, even if this card
        // itself won't be upgrading it. Just to keep the pool the same regardless of whether this card is upgrading
        // the cards it generates.
        // It also is irrelevant if no card that deals damage or gains block upgrades from 2 to 1.
        card = card.ToMutable();
        card.UpgradeInternal();
        return ModelDb.Enchantment<Doodled>().CanEnchant(card);
    }
}