using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.SharedTalents;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Monsters.Figments;

public class TiacauhShredderFigment : SimpleFigment<PowerfulHitTrigger, TiacauhShredderIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_1097_cclmbjk",
        new ActionAnimation("Attack", 0.40, 1.40, 1.95, "Idle") { Timescale = 2.0 });

    public override string CustomVisualPath => Pathfinder.Scene("tiacauh_shredder");

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.Attacking,
        HardTag.FromArknights,
        HardTag.UsesAkEnemy,
        HardTag.Tiacauh
    ];

    protected override int InitialHp => 7;
}