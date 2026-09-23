using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class FlintFigment : SimpleFigment<MahuizzotiaTrigger, PlaceholderTalent, FlintIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_415_flint",
        new ActionAnimation("Skill1", 0.34, "Idle"));

    protected override int InitialHp => 6;
    protected override int MaxHp => 12;
}