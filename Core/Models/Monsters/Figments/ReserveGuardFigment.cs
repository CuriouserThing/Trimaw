using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class ReserveGuardFigment : OneShotFigment<AttackTrigger>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_601_cguard",
        new ActionAnimation("Attack", 0.17, 0.48, 1.11, "Idle"));

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.Attacking,
        HardTag.FromArknights,
        HardTag.UsesAkChar,
        HardTag.ReserveOp
    ];

    protected override int InitialHp => 5;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new AttackRandomIntent(5);
    }
}