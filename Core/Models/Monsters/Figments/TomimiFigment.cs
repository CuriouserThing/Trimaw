using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class TomimiFigment : SimpleFigment<MahuizzotiaTrigger, HuntingBuddyTalent, TomimiIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_411_tomimi",
        new ActionAnimation("Attack", 0.20, 0.60, 1.00, "Idle"));

    protected override int InitialHp => 5;
    protected override int MaxHp => 15;
}