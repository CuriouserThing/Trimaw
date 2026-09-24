using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Powers.CeobePowers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Talents;

public class HuntingBuddyTalent : FigmentTalent
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("target_dummy");
    public override string Icon256Path => Pathfinder.GameIconsDotnet64("target_dummy");

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<AimPower>()];

    private AimPower? Aim => Figment.PetOwner.Creature.GetPower<AimPower>();

    protected internal override MoveParams ModifyMoveParams(MoveParams moveParams, bool dryRun)
    {
        if (Aim is null) return moveParams;

        return new MoveParams(moveParams) { Multiplier = 2 * moveParams.Multiplier };
    }

    protected internal override async Task AfterModifyingMoveParams(MoveParams originalParams, MoveParams newParams)
    {
        if (Aim is { } aim) await PowerCmd.Decrement(aim);
    }
}