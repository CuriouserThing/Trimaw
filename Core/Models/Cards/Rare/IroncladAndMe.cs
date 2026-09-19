using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using Trimaw.Core.Models.Enchantments;

namespace Trimaw.Core.Models.Cards.Rare;

public class IroncladAndMe() : TrimawCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Doodled>();

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.AddGeneratedCardsToCombat(
            [Generate(Owner.Character), Generate(ModelDb.Character<Ironclad>())],
            PileType.Hand, Owner);
    }

    private CardModel Generate(CharacterModel character)
    {
        var card = CardFactory.GetDistinctForCombat(
            Owner,
            character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
                .Where(CanEnchant),
            1,
            Owner.RunState.Rng.CombatCardGeneration).First();
        CardCmd.Enchant<Doodled>(card, 1);
        if (IsUpgraded) CardCmd.Upgrade(card);
        return card;
    }

    private static bool CanEnchant(CardModel card)
    {
        var doodled = ModelDb.Enchantment<Doodled>();
        card = card.ToMutable();
        CardCmd.Upgrade(card);
        return doodled.CanEnchant(card);
    }
}