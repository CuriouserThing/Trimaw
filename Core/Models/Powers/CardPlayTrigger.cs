using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Trimaw.Core.Models.Powers;

public abstract class CardPlayTrigger : FigmentMonoTrigger
{
    protected abstract bool CardMatches(CardPlay cardPlay);

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner.PetOwner && CardMatches(cardPlay))
            GetInternalData<Data>().SeenCards.Add(cardPlay.Card);
        return Task.CompletedTask;
    }

    protected bool CardTriggersMove(CardPlay cardPlay)
    {
        return GetInternalData<Data>().SeenCards.Remove(cardPlay.Card);
    }

    private class Data
    {
        public readonly HashSet<CardModel> SeenCards = [];
    }
}