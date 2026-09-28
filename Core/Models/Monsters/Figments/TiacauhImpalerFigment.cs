using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class TiacauhImpalerFigment : OneShotFigment<MahuizzotiaTrigger, HuntingBuddyTalent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_1094_ccspm",
        new ActionAnimation("Attack", 0.18, 0.46, 0.70, "Idle"));

    protected override int InitialHp => 3;
    protected override int MaxHp => 7;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new AttackLowestIntent(8);
    }
}