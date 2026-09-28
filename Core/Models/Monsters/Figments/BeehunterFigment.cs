using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class BeehunterFigment : SimpleFigment<BearNecessitiesTrigger, AlleyOopTalent, BeehunterIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_137_brownb_kitchen#1",
        new ActionAnimation("Attack", 0.20, 0.44, 0.90, "Idle"));

    protected override int InitialHp => 6;
    protected override int MaxHp => 10;
}