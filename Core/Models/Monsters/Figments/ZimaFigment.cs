using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class ZimaFigment : SimpleFigment<AnyEnergySpentTrigger, ZimaIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_115_headbr_it#1",
        new ActionAnimation("Attack", 0.15, 0.43, 1.00, "Idle"));

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.Attacking,
        HardTag.FromArknights,
        HardTag.UsesAkChar,
        HardTag.UrsusRace
    ];

    protected override int InitialHp => 9;
    protected override int MaxHp => 14;
}