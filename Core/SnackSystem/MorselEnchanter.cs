using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Utils;

namespace Trimaw.Core.SnackSystem;

public abstract class MorselEnchanter : Enchanter
{
    protected MorselEnchanter(Morsel morsel, int amount = 1)
    {
        Morsel = morsel;
        Amount = amount;
    }

    public Morsel Morsel { get; }
}

public class MorselEnchanter<T>(Morsel morsel, int amount = 1) : MorselEnchanter(morsel, amount)
    where T : EnchantmentModel
{
    public override EnchantmentModel CanonicalModel => ModelDb.Enchantment<T>();

    public override EnchantmentModel? Enchant(CardModel card)
    {
        return CardCmd.Enchant<T>(card, Amount);
    }
}