using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Utils;

namespace Trimaw.Core.SnackSystem;

internal class MorselEnchanter<T>(Morsel morsel, int amount = 1)
    : Enchanter<T>(amount), IMorselEnchanter where T : EnchantmentModel
{
    public Morsel Morsel => morsel;
}