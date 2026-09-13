using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;

namespace Trimaw.Core.Utils;

internal class Enchanter<T>(int amount = 1) : IEnchanter, IEquatable<Enchanter<T>> where T : EnchantmentModel
{
    public Predicate<CardModel>? CanPotentiallyEnchantPredicate { get; init; }
    public string Name => typeof(T).Name;

    public int Amount { get; } = amount;

    public EnchantmentModel CanonicalModel => ModelDb.Enchantment<T>();

    public bool CanPotentiallyEnchant(CardModel card)
    {
        return CanPotentiallyEnchantPredicate is null || CanPotentiallyEnchantPredicate(card);
    }

    public EnchantmentModel? Enchant(CardModel card)
    {
        return CardCmd.Enchant<T>(card, Amount);
    }

    public override string ToString()
    {
        return Amount == 1 ? $"{Name}" : $"{Name} {Amount}";
    }

    #region Equality

    public bool Equals(Enchanter<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Amount == other.Amount;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Enchanter<T>)obj);
    }

    public override int GetHashCode()
    {
        return Amount;
    }

    #endregion
}