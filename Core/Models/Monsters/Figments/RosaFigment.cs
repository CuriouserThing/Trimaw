using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class RosaFigment : SimpleFigment<AimShareTalent, RosaIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_197_poca_epoque#12",
        new ActionAnimation("Attack", 0.20, 0.56, 2.17, "Idle"));

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.Attacking,
        HardTag.FromArknights,
        HardTag.UsesAkChar,
        HardTag.UrsusRace
    ];

    protected override int InitialHp => 3;
}