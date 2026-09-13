using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class Spiced : TrimawEnchantment
{
    public override bool HasExtraCardText => true;
    public override string Icon64Path => Pathfinder.NotoEmoji64("ginger");

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card != Card || Card.CombatState is null) return;

        var creature = Card.Owner.Creature;
        var vigor = Amount * Card.CombatState.Creatures.Count;
        await PowerCmd.Apply<VigorPower>(choiceContext, creature, vigor, creature, null);
    }
}