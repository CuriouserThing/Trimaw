using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Triggers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Monsters.Figments;

public class TiacauhShredderFigment : SimpleFigment<MahuizzotiaTrigger, PlaceholderTalent, TiacauhShredderIntent>
{
    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_1097_cclmbjk",
        new ActionAnimation("Attack", 0.40, 1.40, 1.95, "Idle") { Timescale = 2.0 });

    public override string CustomVisualPath => Pathfinder.Scene("tiacauh_shredder");

    protected override int InitialHp => 8;
    protected override int MaxHp => 12;
}