using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Animation;

namespace Trimaw.Core.Commands;

/// <summary>
///     Commands that don't fit elsewhere.
/// </summary>
public static class TrimawCmd
{
    public static TrimawAttackCommand Attack(CardModel card, CardPlay? cardPlay, decimal damagePerHit)
    {
        return new TrimawAttackCommand(card, cardPlay, damagePerHit);
    }
}