using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class GopnikFigment : SimpleFigment<BearNecessitiesTrigger, BusinessAsUsualTalent, GopnikIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_2002_bearmi",
        new ActionAnimation("Attack", 0.30, 1.27, 2.60, "Idle") { Timescale = 2.0 });

    protected override int InitialHp => 9;
    protected override int MaxHp => 23;

    protected override int InitialTalentPowerAmount => 110;
}