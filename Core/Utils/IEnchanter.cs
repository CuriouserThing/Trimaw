using MegaCrit.Sts2.Core.Models;

namespace Trimaw.Core.Utils;

public interface IEnchanter
{
    public string Name { get; }

    public int Amount { get; }

    public EnchantmentModel CanonicalModel { get; }

    /// <summary>
    ///     Additional restriction on top of the <see cref="EnchantmentModel" />'s own restrictions.
    /// </summary>
    public bool CanPotentiallyEnchant(CardModel card)
    {
        return true;
    }

    public bool CanEnchant(CardModel card)
    {
        return CanPotentiallyEnchant(card) && CanonicalModel.CanEnchant(card);
    }

    public EnchantmentModel? Enchant(CardModel card);
}