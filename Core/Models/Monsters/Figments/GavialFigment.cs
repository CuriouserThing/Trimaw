using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class GavialFigment : Figment<ImaginaryShieldPower, MahuizzotiaTrigger, PromotionTalent>
{
    public static string AttackBId => "Skill_2_Loop";

    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_1026_gvial2",
            new ActionAnimation("Skill_3", 0.43, "Skill_2_Idle")
            {
                StartKey = 0.12,
                EndId = "Skill_2_Begin",
                EndKey = 0 // move back as soon as she starts planting her saw
            })
        .WithSecondaryAnimation(new ActionAnimation(AttackBId, 0.18, 0.40, 1.00, "Skill_2_Idle"));

    protected override int InitialHp => 8;
    protected override int MaxHp => 18;

    protected override async Task<FigmentIntent?> ReadyNextIntent(PlayerChoiceContext choiceContext)
    {
        switch (MovesUsed)
        {
            case 0:
                return new GavialAttackAIntent();
            case 1:
                await PowerCmd.Apply<MahuizzotiaTrigger>(choiceContext, Creature, 1, Creature, null);
                return new GavialAttackBIntent();
            default:
                return null;
        }
    }
}