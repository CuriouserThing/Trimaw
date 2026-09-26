using MegaCrit.Sts2.Core.Entities.Powers;
using Trimaw.Core.ImaginationSystem;

namespace Trimaw.Core.Models.Powers.Talents;

public class PromotionTalent : FigmentTalent
{
    public override PowerStackType StackType => PowerStackType.Single;

    protected override bool ShouldRemoveWhenNoMoves => true;

    protected internal override MoveParams ModifyMoveParams(MoveParams moveParams, bool dryRun)
    {
        var mult = Figment.PetOwner.RunState.Act.Index switch
        {
            0 => 1,
            1 => 2,
            _ => 3
        };

        return new MoveParams(moveParams) { Multiplier = mult * moveParams.Multiplier };
    }
}