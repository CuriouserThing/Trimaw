using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Talents;

public class ScrapMetalTalent : FigmentTalent
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("gears");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("gears");

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target,
        DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner) return;

        Flash();
        await PowerCmd.Apply<ArtifactPower>(choiceContext, Figment.PetOwner.Creature, 1, Owner, null);
        await PowerCmd.Decrement(this);
    }
}