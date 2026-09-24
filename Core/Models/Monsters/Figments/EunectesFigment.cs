using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class EunectesFigment : Figment<ImaginaryShieldPower, OneCostTrigger, PlaceholderTalent>
{
    private const int MoveCount = 2;

    public static string SummonId => "Skill_2_Begin";

    // Atlas for this skeleton adjusted to remove most of Big Ugly (and hopefully none of Zumama herself)
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_416_zumama",
            new ActionAnimation("Attack", 0.17, 0.50, 0.75, "Idle"))
        .WithSecondaryAnimation(new ActionAnimation(SummonId, 0.16, 0.70, 0.70, null) { CutoffTime = 0.70 });

    protected override int InitialHp => 7;
    protected override int MaxHp => 16;

    protected override int InitialTriggerPowerAmount => 3;

    protected override void CountMovesRemaining(out uint min, out uint? max)
    {
        min = (uint)Math.Max(0, MoveCount - MovesUsed);
        max = min;
    }

    protected override FigmentIntent? GetNextFigmentIntent()
    {
        return MovesUsed switch
        {
            0 => new EunectesBuildIntent(),
            1 => new EunectesSummonIntent(),
            _ => null
        };
    }
}