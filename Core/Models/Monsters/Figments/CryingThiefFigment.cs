using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class CryingThiefFigment : SimpleFigment<FetchTrigger, BusinessAsUsualTalent, CryingThiefIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_2034_sythef",
        new ActionAnimation("Attack", 0.30, 0.76, 1.20, "Idle"));

    protected override int InitialHp => 3;
    protected override int MaxHp => 6;

    protected override int InitialTriggerPowerAmount => 5;
    protected override int InitialTalentPowerAmount => 150;
}