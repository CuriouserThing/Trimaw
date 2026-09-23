using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class ReserveSupporterFigment : OneShotFigment<BackToBasicsTrigger, SurviveAtOneTalent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_606_csuppo",
        new ActionAnimation("Attack", 0.20, 0.41, 0.91, "Idle"));

    protected override int InitialHp => 5;
    protected override int MaxHp => 9;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new HealIntent(5);
    }
}