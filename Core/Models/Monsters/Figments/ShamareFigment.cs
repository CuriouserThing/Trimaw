using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class ShamareFigment : SimpleFigment<TargetTrigger, ShamareIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_254_vodfox_witch#2",
        new ActionAnimation("Attack", 0.15, 0.53, 1.00, "Idle"));

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.FromArknights,
        HardTag.UsesAkChar
    ];

    protected override int InitialHp => 4;
    protected override int MaxHp => 7;

    protected override int InitialTalentPowerAmount => 2;
}