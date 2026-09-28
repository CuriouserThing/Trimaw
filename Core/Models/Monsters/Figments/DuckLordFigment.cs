using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Monsters.Figments;

public class DuckLordFigment : Figment<PhantasmagoriaPower, SkillCheckTrigger, BusinessAsUsualTalent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_2001_duckmi",
        new ActionAnimation("Move", 0, null));

    protected override int InitialHp => 4;
    protected override int MaxHp => 9;

    protected override int InitialTriggerPowerAmount => 5;
    protected override int InitialTalentPowerAmount => 40;

    protected override Task<FigmentIntent?> ReadyNextIntent(PlayerChoiceContext choiceContext)
    {
        return MovesUsed switch
        {
            0 => Task.FromResult<FigmentIntent?>(new DuckLordIntent()),
            _ => Task.FromResult<FigmentIntent?>(null)
        };
    }
}