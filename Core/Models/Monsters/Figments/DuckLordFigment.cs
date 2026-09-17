using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.UniqueTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class DuckLordFigment : SimpleFigment<DuckLordTalent, DuckLordIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_2001_duckmi",
        new ActionAnimation("Move", 0, null));

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.FromArknights,
        HardTag.UsesAkEnemy,
        HardTag.DuckLordAssociate
    ];

    protected override int InitialHp => 5;
    protected override int MaxHp => 14;

    protected override int InitialTalentPowerAmount => 40;
}