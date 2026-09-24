using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class TiacauhRitualistFigment : OneShotFigment<MahuizzotiaTrigger, ArtsAssimilationTalent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_1096_ccwitch",
        new ActionAnimation("Attack_01", 1.20, 1.61, 1.80, "Idle") { Timescale = 1.5 });

    protected override int InitialHp => 5;
    protected override int MaxHp => 9;

    protected override int InitialTalentPowerAmount => 30;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new AttackAnyIntent(10);
    }
}