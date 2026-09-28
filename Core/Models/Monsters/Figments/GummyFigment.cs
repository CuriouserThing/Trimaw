using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class GummyFigment : SimpleFigment<BearNecessitiesTrigger, PlaytimeTalent, GummyIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_196_sunbr_summer#1",
        new ActionAnimation("Skill", 0.20, 0.50, 1.05, "Idle"));

    protected override int InitialHp => 7;
    protected override int MaxHp => 18;
}