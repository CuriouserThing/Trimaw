using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class ReserveGuardFigment : OneShotFigment<BackToBasicsTrigger, PromotionTalent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_601_cguard",
        new ActionAnimation("Attack", 0.17, 0.48, 1.11, "Idle"));

    protected override int InitialHp => 7;
    protected override int MaxHp => 14;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new AttackAnyIntent(5);
    }
}