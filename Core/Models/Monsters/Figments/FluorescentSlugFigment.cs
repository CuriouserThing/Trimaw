using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.SharedTalents;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Monsters.Figments;

public class FluorescentSlugFigment : SimpleFigment<PrepTrigger, FluorescentSlugIntent>
{
    // This little guy does actually have a Start animation!
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("enemy_6008_mtslms",
        new ActionAnimation("Move", 0.40, "Idle"));

    public override string CustomVisualPath => Pathfinder.Scene("sluggy");

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.FromArknights,
        HardTag.UsesAkEnemy,
        HardTag.Slug
    ];

    protected override int InitialHp => 3;
    protected override int MaxHp => 5;
}