using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Animation;

namespace Trimaw.Core.Commands;

/// <summary>
///     Commands that don't fit elsewhere.
/// </summary>
public static class TrimawCmd
{
    public static Task MakeSuperSpecial(CardModel card)
    {
        // It's not impossible to envision some fx here, if it's in hand
        MainFile.Logger.Info($"Making {card} Super Special.");
        MainFile.CombatManagerFactory.GetOrCreate(card.Owner).MakeSuperSpecial(card);
        return Task.CompletedTask;
    }

    public static TrimawAttackCommand Attack(CardModel card, CardPlay? cardPlay, decimal damagePerHit)
    {
        return new TrimawAttackCommand(card, cardPlay, damagePerHit);
    }
}