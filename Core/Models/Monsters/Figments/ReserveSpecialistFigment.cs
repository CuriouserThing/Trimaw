using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class ReserveSpecialistFigment : OneShotFigment<EotTrigger>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_607_cspec",
        new ActionAnimation("Attack", 0.17, 0.32, 0.68, "Idle"));

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.Attacking,
        HardTag.FromArknights,
        HardTag.UsesAkChar,
        HardTag.ReserveOp
    ];

    protected override int InitialHp => 6;
    protected override int MaxHp => 11;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new AttackLowestIntent(4);
    }
}