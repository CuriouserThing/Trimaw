using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Talents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class FattyFigment : SimpleFigment<PlaceholderTrigger, BusinessAsUsualTalent, FattyIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_2085_skzjxd",
        new ActionAnimation("Special", 0.20, 0.45, 1.00, "Idle")
        {
            Timescale = 1.5,
            CutoffTime = 2.00
        });

    protected override int InitialHp => 8;
    protected override int MaxHp => 16;

    protected override int InitialTalentPowerAmount => 70;
}