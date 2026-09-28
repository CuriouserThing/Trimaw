using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class ReserveSniperFigment : OneShotFigment<BackToBasicsTrigger, AlleyOopTalent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_603_csnipe",
        new ActionAnimation("Attack", 0.10, 0.54, 0.90, "Idle"));

    protected override int InitialHp => 3;
    protected override int MaxHp => 6;

    protected override int InitialTriggerPowerAmount => 1;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new AttackLowestIntent(3);
    }
}