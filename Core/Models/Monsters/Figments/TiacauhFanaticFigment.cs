using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class TiacauhFanaticFigment : SimpleFigment<MahuizzotiaTrigger, PlaceholderTalent, TiacauhFanaticIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_1095_ccripr",
        new ActionAnimation("Attack", 0.34, "Idle"));

    protected override int InitialHp => 6;
    protected override int MaxHp => 11;
}