using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class Xxl : TrimawEnchantment
{
    public override bool ShowAmount => Amount > 1;
    public override string Icon64Path => Pathfinder.NotoEmoji64("negative_squared_cross_mark");

    public override bool CanEnchant(CardModel card)
    {
        return base.CanEnchant(card) && card.CanonicalKeywords.Contains(CardKeyword.Exhaust);
    }

    protected override void OnEnchant()
    {
        if (Card.Keywords.Contains(CardKeyword.Exhaust)) CardCmd.RemoveKeyword(Card, CardKeyword.Exhaust);
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card != Card ||
            Status == EnchantmentStatus.Disabled ||
            !Card.CanonicalKeywords.Contains(CardKeyword.Exhaust))
            return Task.CompletedTask;

        Amount -= 1;
        if (Amount < 1)
        {
            CardCmd.ApplyKeyword(Card, CardKeyword.Exhaust);
            Status = EnchantmentStatus.Disabled;
        }

        return Task.CompletedTask;
    }
}