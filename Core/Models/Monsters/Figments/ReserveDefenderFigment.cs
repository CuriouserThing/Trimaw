using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.SharedIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class ReserveDefenderFigment : OneShotFigment<BackToBasicsTrigger, PlaceholderTalent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_602_cdfend",
        new ActionAnimation("Attack", 0.28, 0.66, 1.05, "Idle"));

    protected override int InitialHp => 7;
    protected override int MaxHp => 18;

    protected override FigmentIntent GetFigmentIntent()
    {
        return new BlockIntent(3);
    }
}