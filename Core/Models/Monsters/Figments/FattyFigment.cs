using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class FattyFigment : SimpleFigment<EotGoldTrigger, FattyIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_2085_skzjxd",
        new ActionAnimation("Special", 0.20, 0.45, 1.00, "Idle")
        {
            Timescale = 1.5,
            CutoffTime = 2.00
        });

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.FromArknights,
        HardTag.UsesAkEnemy,
        HardTag.DuckLordAssociate
    ];

    protected override int InitialHp => 6;

    protected override int InitialTalentPowerAmount => 70;
}