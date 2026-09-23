using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Triggers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Monsters.Figments;

public class BerrySlugFigment : SimpleFigment<FieldRationTrigger, PlaceholderTalent, BerrySlugIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_10002_trtrsl",
        new ActionAnimation("Move", 0.25, 0.50, 0.75, "Idle"));

    public override string CustomVisualPath => Pathfinder.Scene("sluggy");

    protected override int InitialHp => 3;
    protected override int MaxHp => 5;
}