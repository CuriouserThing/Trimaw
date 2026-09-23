using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Talents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class VulcanFigment : SimpleFigment<PlaceholderTrigger, VulcanTalent, VulcanIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_163_hpsts_boc#10",
        new ActionAnimation("Skill", 0.44, 1.00, 1.38, "Idle"));

    protected override int InitialHp => 8;
    protected override int MaxHp => 20;
}