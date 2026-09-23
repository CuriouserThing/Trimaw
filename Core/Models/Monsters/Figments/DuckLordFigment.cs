using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Talents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class DuckLordFigment : SimpleFigment<PlaceholderTrigger, BusinessAsUsualTalent, DuckLordIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_2001_duckmi",
        new ActionAnimation("Move", 0, null));

    protected override int InitialHp => 5;
    protected override int MaxHp => 14;

    protected override int InitialTalentPowerAmount => 40;
}