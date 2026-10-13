using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Hooks;

public interface IEnchantListener
{
    IEnchanter ModifyEnchanter(CardModel card, IEnchanter enchanter);

    Task AfterModifyingEnchanter(CardModel card, IEnchanter enchanter);
}