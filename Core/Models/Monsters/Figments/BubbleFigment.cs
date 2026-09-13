using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class BubbleFigment : SimpleFigment<AttackAttackerTrigger, BubbleIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_381_bubble",
        new ActionAnimation("Skill_Start", 0.14, 0.47, null, "Skill_Loop"));

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.FromArknights,
        HardTag.UsesAkChar
    ];

    protected override int InitialHp => 7;
}