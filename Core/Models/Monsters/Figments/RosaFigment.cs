using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class RosaFigment : SimpleFigment<BearNecessitiesTrigger, HuntingBuddyTalent, RosaIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_197_poca_epoque#12",
        new ActionAnimation("Attack", 0.20, 0.56, 2.17, "Idle"));

    protected override int InitialHp => 4;
    protected override int MaxHp => 6;
}