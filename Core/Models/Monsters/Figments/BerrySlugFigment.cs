using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.SharedTalents;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Monsters.Figments;

public class BerrySlugFigment : SimpleFigment<PrepTrigger, BerrySlugIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_10002_trtrsl",
        new ActionAnimation("Move", 0.25, 0.50, 0.75, "Idle"));

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