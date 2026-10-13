using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Hooks;

public interface IEnchantListener
{
    Enchanter ModifyEnchanter(CardModel card, Enchanter enchanter);

    Task AfterModifyingEnchanter(CardModel card, Enchanter enchanter);
}