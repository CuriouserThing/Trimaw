using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class IstinaFigment : SimpleFigment<BearNecessitiesTrigger, ArtsAssimilationTalent, IstinaIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_195_glassb_kitchen#1",
        new ActionAnimation("Attack", 0.20, 0.45, 1.12, "Idle"));

    protected override int InitialHp => 4;
    protected override int MaxHp => 7;
}