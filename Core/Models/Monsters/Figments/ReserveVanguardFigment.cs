using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class ReserveVanguardFigment : OneShotFigment<BackToBasicsTrigger, AlleyOopTalent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_600_cpione",
        new ActionAnimation("Attack", 0.17, 0.49, 0.90, "Idle"));

    protected override int InitialHp => 8;
    protected override int MaxHp => 12;

    protected override int InitialTriggerPowerAmount => 1;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new FastenIntent(2);
    }
}