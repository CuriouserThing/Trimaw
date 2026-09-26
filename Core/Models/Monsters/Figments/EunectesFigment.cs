using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
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

    protected override async Task<FigmentIntent?> ReadyNextIntent(PlayerChoiceContext choiceContext)
    {
        switch (MovesUsed)
        {
            case 0:
                return new EunectesBuildIntent();
            case 1:
                await PowerCmd.Apply<AnyCardTrigger>(choiceContext, Creature, 1, Creature, null);
                return new EunectesSummonIntent();
            default:
                return null;
        }
    }
}