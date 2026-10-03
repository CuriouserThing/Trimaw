using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class CerberusFormPower : TrimawPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("triforce");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("triforce");

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner.Creature != Owner || card.Type != CardType.Attack) return playCount;

        var attacksPlayed = CombatManager.Instance.History.CardPlaysFinished
            .Count(c => c.HappenedThisTurn(CombatState) && c.CardPlay.Card.Type == CardType.Attack &&
                        c.CardPlay.IsFirstInSeries);
        return attacksPlayed < Amount ? playCount + 2 : playCount;
    }
}