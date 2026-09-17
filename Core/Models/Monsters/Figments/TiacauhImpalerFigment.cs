using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class TiacauhImpalerFigment : OneShotFigment<AimShareTalent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_1094_ccspm",
        new ActionAnimation("Attack", 0.18, 0.46, 0.70, "Idle"));

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.Attacking,
        HardTag.FromArknights,
        HardTag.UsesAkEnemy,
        HardTag.Tiacauh
    ];

    protected override int InitialHp => 6;
    protected override int MaxHp => 12;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new AttackLowestIntent(8);
    }
}