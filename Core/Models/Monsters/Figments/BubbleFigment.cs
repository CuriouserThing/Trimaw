using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class BubbleFigment : SimpleFigment<AttackAttackerTrigger, PlaceholderTalent, BubbleIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_381_bubble",
        new ActionAnimation("Skill_Start", 0.14, 0.47, null, "Skill_Loop"));

    protected override int InitialHp => 8;
    protected override int MaxHp => 19;
}