using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class BeehunterFigment : SimpleFigment<AnyEnergySpentTrigger, BeehunterIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_137_brownb_kitchen#1",
        new ActionAnimation("Attack", 0.20, 0.44, 0.90, "Idle"));

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.FromArknights,
        HardTag.UsesAkChar,
        HardTag.UrsusRace
    ];

    protected override int InitialHp => 6;
    protected override int MaxHp => 10;
}