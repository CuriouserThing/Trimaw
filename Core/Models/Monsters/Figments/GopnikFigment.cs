using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class GopnikFigment : SimpleFigment<EotGoldTrigger, GopnikIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_2002_bearmi",
        new ActionAnimation("Attack", 0.30, 1.27, 2.60, "Idle") { Timescale = 2.0 });

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.Attacking,
        HardTag.FromArknights,
        HardTag.UsesAkEnemy,
        HardTag.UrsusRace,
        HardTag.DuckLordAssociate
    ];

    protected override int InitialHp => 9;
    protected override int MaxHp => 23;

    protected override int InitialTalentPowerAmount => 110;
}