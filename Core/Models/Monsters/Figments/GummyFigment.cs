using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class GummyFigment : SimpleFigment<AllEnergySpentTrigger, GummyIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_196_sunbr_summer#1",
        new ActionAnimation("Skill", 0.20, 0.50, 1.05, "Idle"));

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.FromArknights,
        HardTag.UsesAkChar,
        HardTag.UrsusRace
    ];

    protected override int InitialHp => 7;
    protected override int MaxHp => 18;
}