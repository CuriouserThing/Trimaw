using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class ReserveVanguardFigment : OneShotFigment<EotTrigger>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_600_cpione",
        new ActionAnimation("Attack", 0.17, 0.49, 0.90, "Idle"));

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.FromArknights,
        HardTag.UsesAkChar,
        HardTag.ReserveOp
    ];

    protected override int InitialHp => 8;
    protected override int MaxHp => 12;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new BlockIntent(3);
    }
}