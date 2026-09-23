using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Talents;

public class SurviveAtOneTalent : FigmentTalent
{
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string Icon64Path => Pathfinder.NotoEmoji64("man_fairy_medium_dark_skin_tone");
    public override string Icon256Path => Pathfinder.NotoEmoji256("man_fairy_medium_dark_skin_tone");

    public override bool ShouldDie(Creature creature)
    {
        if (creature != Owner) return true;

        return false;
    }

    public override async Task AfterPreventingDeath(Creature creature)
    {
        Flash();
        await CreatureCmd.Heal(creature, 1);
        await PowerCmd.Decrement(this);
    }
}