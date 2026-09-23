using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class SesaFigment : SimpleFigment<OneCostTrigger, PlaceholderTalent, SesaIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_379_sesa",
        new ActionAnimation("Skill", 0.30, 1.63, 2.10, "Idle") { Timescale = 1.5 });

    protected override int InitialHp => 5;
    protected override int MaxHp => 10;

    protected override int InitialTalentPowerAmount => 2;
}