using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using Trimaw.Core.Hooks;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class StoneDiseasePower : TrimawPower, IEnchantListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public IEnchanter ModifyEnchanter(CardModel card, IEnchanter enchanter)
    {
        if (card.Owner.Creature != Owner ||
            card.Type != CardType.Attack ||
            enchanter.CanonicalModel is Instinct) return enchanter;

        IEnchanter newEnchanter = new Enchanter<Instinct>();
        return newEnchanter.CanEnchant(card) ? newEnchanter : enchanter;
    }

    public Task AfterModifyingEnchanter(CardModel card, IEnchanter enchanter)
    {
        Flash();
        return Task.CompletedTask;
    }
}