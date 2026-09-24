using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class ReserveCasterFigment : OneShotFigment<BackToBasicsTrigger, ArtsAssimilationTalent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_604_ccast",
        new ActionAnimation("Attack", 0.15, 0.52, 0.92, "Idle"));

    protected override int InitialHp => 4;
    protected override int MaxHp => 6;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new AttackAnyIntent(5);
    }
}