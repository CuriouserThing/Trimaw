using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.SharedTalents;

public class HpLossTrigger : SingleTriggerTalent
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.NotoEmoji64("police_cars_revolving_light");
    public override string Icon256Path => Pathfinder.NotoEmoji256("police_cars_revolving_light");

    public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        if (delta > 0 || Owner.PetOwner is not { } petOwner) return;

        if (creature == petOwner.Creature ||
            (creature.PetOwner == petOwner && creature.Monster is Figment))
            await TriggerMove(null);
    }
}