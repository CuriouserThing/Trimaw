using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class CerberusFormPower : TrimawPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner.Creature != Owner) return playCount;

        var attacksPlayed = CombatManager.Instance.History.CardPlaysFinished
            .Count(c => c.CardPlay.Card.Type == CardType.Attack && c.CardPlay.IsFirstInSeries);
        return attacksPlayed < Amount ? playCount + 2 : playCount;
    }
}