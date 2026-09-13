using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.UniqueTalents;

public class VulcanTalent : FigmentTalentPower
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("fireplace");
    public override string Icon256Path => Pathfinder.GameIconsDotnet64("fireplace");

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(1, ValueProp.Unpowered)];

    public override bool ShouldFlush(Player player)
    {
        return player != Owner.PetOwner;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (Owner.PetOwner is not { } petOwner || !participants.Contains(petOwner.Creature)) return;

        Flash();
        var cards = PileType.Hand.GetPile(petOwner).Cards;
        if (cards.Count == 0) return;
        var block = cards.Count * DynamicVars.Block.IntValue;
        await CreatureCmd.GainBlock(petOwner.Creature, block, ValueProp.Unpowered, null);
    }
}