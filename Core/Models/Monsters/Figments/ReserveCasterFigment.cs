using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class ReserveCasterFigment : OneShotFigment<SurviveAtOneTalent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_604_ccast",
        new ActionAnimation("Attack", 0.15, 0.52, 0.92, "Idle"));

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.Attacking,
        HardTag.FromArknights,
        HardTag.UsesAkChar,
        HardTag.ReserveOp
    ];

    protected override int InitialHp => 4;
    protected override int MaxHp => 6;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new AttackRandomIntent(5);
    }
}