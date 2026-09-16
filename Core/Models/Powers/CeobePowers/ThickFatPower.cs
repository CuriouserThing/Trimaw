using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Cards;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class ThickFatPower : TrimawPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Player.Creature != Owner || cardPlay.Card is not SnackCard) return;

        await CreatureCmd.GainBlock(cardPlay.Player.Creature, Amount, ValueProp.Unpowered, cardPlay);
    }
}