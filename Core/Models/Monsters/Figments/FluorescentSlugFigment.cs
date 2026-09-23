using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Triggers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Monsters.Figments;

public class FluorescentSlugFigment : SimpleFigment<FieldRationTrigger, PlaceholderTalent, FluorescentSlugIntent>
{
    // This little guy does actually have a Start animation!
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("enemy_6008_mtslms",
        new ActionAnimation("Move", 0.40, "Idle"));

    public override string CustomVisualPath => Pathfinder.Scene("sluggy");

    protected override int InitialHp => 3;
    protected override int MaxHp => 5;
}