using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters;

namespace Trimaw.Core.Models.Powers;

public abstract class FigmentDefensePower : FigmentPower
{
    private protected FigmentDefensePower()
    {
    }

    public override PowerStackType StackType => PowerStackType.Single;

    public override Creature ModifyUnblockedDamageTarget(Creature target, decimal amount, ValueProp props,
        Creature? dealer)
    {
        var figment = Owner;

        // Do not redirect 0 damage! There are multiple ways 0 damage can squeak through, so it's easier to say no to all.
        // As it stands, we can't easily distinguish between those and 0 damage sneaking past full block.
        if (amount == 0) return target;

        // Do not redirect damage from THIS figment! Why would it want to hurt you, though? :(
        if (dealer == figment) return target;

        // Run any checks needed to bring behavior in line with Osty's Die For You.
        // NOTE: This can change, but Osty-like behavior follows a practice of least surprise for the player.  
        if (figment.IsDead || !props.IsPoweredAttack()) return target;

        // Is the target this pet's owner? Redirect if so.
        if (target == figment.PetOwner?.Creature) return figment;

        // Is the target not a pet, or a pet with a different owner? Keep target if so.
        if (target.PetOwner != figment.PetOwner) return target;

        // Does the target have less HP? Redirect if so.
        if (target.CurrentHp < figment.CurrentHp) return figment;

        // Does the target have *more* HP? Keep target if so.
        if (target.CurrentHp > figment.CurrentHp) return target;

        // Redirect if target is older; keep target otherwise.
        return GetTimestamp(target) < GetTimestamp(figment) ? figment : target;
    }

    private static int GetTimestamp(Creature target)
    {
        return (target.Monster as Figment)?.Timestamp ?? 0;
    }
}