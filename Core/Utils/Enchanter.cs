using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.Models;

namespace Trimaw.Core.Utils;

public abstract class Enchanter
{
    public abstract EnchantmentModel CanonicalModel { get; }

    public int Amount { get; init; } = 1;

    public bool CanEnchantCloneOf(CardModel card)
    {
        var canonicalCard = card.CanonicalInstance;
        if (!CanonicalModel.CanEnchant(canonicalCard) || PreventedFromEnchanting(canonicalCard)) return false;
        var oldEnchantment = card.Enchantment;
        if (oldEnchantment is null || oldEnchantment.Status == EnchantmentStatus.Disabled) return true;
        return CanStackEnchantment(oldEnchantment);
    }

    public abstract EnchantmentModel? Enchant(CardModel card);

    protected virtual bool PreventedFromEnchanting(CardModel card)
    {
        return false;
    }

    protected virtual bool CanStackEnchantment(EnchantmentModel enchantment)
    {
        return false;
    }

    public override string ToString()
    {
        var name = CanonicalModel.GetType().Name;
        return Amount == 1 ? name : $"{name} {Amount}";
    }
}

public class Enchanter<T> : Enchanter where T : EnchantmentModel
{
    public Enchanter(int amount = 1)
    {
        Amount = amount;
    }

    public Predicate<CardModel> PreventedFromEnchantingPredicate { get; init; } = _ => false;

    public bool CanStackSameEnchantment { get; init; } = false;

    public override EnchantmentModel CanonicalModel => ModelDb.Enchantment<T>();

    public override EnchantmentModel? Enchant(CardModel card)
    {
        var oldEnchantment = card.Enchantment;
        var amount = Amount;
        if (oldEnchantment is not null)
        {
            if (CanStackEnchantment(oldEnchantment)) amount += oldEnchantment.Amount;
            CardCmd.ClearEnchantment(card);
        }

        return CardCmd.Enchant<T>(card, amount);
    }

    protected override bool PreventedFromEnchanting(CardModel card)
    {
        return PreventedFromEnchantingPredicate(card);
    }

    protected override bool CanStackEnchantment(EnchantmentModel enchantment)
    {
        return enchantment is T && (CanStackSameEnchantment || enchantment.IsStackable);
    }
}