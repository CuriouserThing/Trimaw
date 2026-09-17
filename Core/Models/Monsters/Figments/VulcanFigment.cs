using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.UniqueTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class VulcanFigment : SimpleFigment<VulcanTalent, VulcanIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_163_hpsts_boc#10",
        new ActionAnimation("Skill", 0.44, 1.00, 1.38, "Idle"));

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.FromArknights,
        HardTag.UsesAkChar
    ];

    protected override int InitialHp => 8;
    protected override int MaxHp => 20;
}