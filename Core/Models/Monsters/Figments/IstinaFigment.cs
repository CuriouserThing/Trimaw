using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.SharedTalents;

namespace Trimaw.Core.Models.Monsters.Figments;

public class IstinaFigment : SimpleFigment<CardsDrawnTrigger, IstinaIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.BuildWithStart("char_195_glassb_kitchen#1",
        new ActionAnimation("Attack", 0.20, 0.45, 1.12, "Idle"));

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.FromArknights,
        HardTag.UsesAkChar,
        HardTag.UrsusRace
    ];

    protected override int InitialHp => 4;
    protected override int MaxHp => 7;

    protected override int InitialTalentPowerAmount => 3;
}