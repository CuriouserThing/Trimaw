using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Powers.Triggers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class Slimy : TrimawEnchantment
{
    public override bool HasExtraCardText => true;
    public override string Icon64Path => Pathfinder.GameIconsDotnet64("gloop");

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card != Card || Card.CombatState is null) return;

        await ImaginationCmd.ImagineRandom<FieldRationTrigger>(choiceContext, Card.Owner);
    }
}