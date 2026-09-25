using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class ReserveMedicFigment : OneShotFigment<BackToBasicsTrigger, FairyInABubbleTalent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_605_cmedic",
        new ActionAnimation("Attack", 0.17, 0.37, 0.81, "Idle"));

    protected override int InitialHp => 4;
    protected override int MaxHp => 9;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new HealIntent(5);
    }
}