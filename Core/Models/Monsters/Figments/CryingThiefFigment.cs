using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Talents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class CryingThiefFigment : SimpleFigment<PlaceholderTrigger, BusinessAsUsualTalent, CryingThiefIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_2034_sythef",
        new ActionAnimation("Attack", 0.30, 0.76, 1.20, "Idle"));

    protected override int InitialHp => 4;
    protected override int MaxHp => 8;

    protected override int InitialTalentPowerAmount => 150;
}