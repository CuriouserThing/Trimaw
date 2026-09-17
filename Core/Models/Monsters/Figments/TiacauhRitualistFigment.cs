using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class TiacauhRitualistFigment : OneShotFigment<AimShareTalent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_1096_ccwitch",
        new ActionAnimation("Attack_01", 1.20, 1.61, 1.80, "Idle") { Timescale = 1.5 });

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.Attacking,
        HardTag.FromArknights,
        HardTag.UsesAkEnemy,
        HardTag.Tiacauh
    ];

    protected override int InitialHp => 5;
    protected override int MaxHp => 9;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new AttackRandomIntent(10);
    }
}