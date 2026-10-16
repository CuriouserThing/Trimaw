using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using Trimaw.Core.Models.Monsters;

namespace Trimaw.Core.ImaginationSystem;

public abstract class MoveContext(Player petOwner, MoveParams moveParams)
{
    public abstract Figment MoveUser { get; }

    public Player PetOwner { get; } = petOwner;

    public MoveParams Params { get; } = moveParams;

    public ICombatState CombatState => MoveUser.CombatState;

    public MegaAnimationState? AnimationState =>
        MoveUser.Creature.GetCreatureNode()?.Visuals.SpineBody?.TryGetAnimationState();

    /// <summary>
    ///     <c>(BaseAmount + Addend) * Multiplier</c>.
    /// </summary>
    public decimal GetAmount()
    {
        return TransformAmount(Params.BaseAmount);
    }

    /// <summary>
    ///     <c>(Amount + Addend) * Multiplier</c>. Ignores <see cref="MoveParams.BaseAmount" />.
    /// </summary>
    public decimal TransformAmount(decimal amount)
    {
        return (amount + Params.Addend) * Params.Multiplier;
    }

    /// <summary>
    ///     Creature for the move to target. A non-null target may come from either the caller or a random selection. Null
    ///     implies that no valid target is available.
    /// </summary>
    /// <param name="validTargetPredicate">
    ///     Optional predicate to narrow down any random selection from the set of all hittable
    ///     enemies.
    /// </param>
    public Creature? GetEnemyTarget(Predicate<Creature>? validTargetPredicate = null)
    {
        if (Params.EnemyTarget is { IsAlive: true } target) return target;

        IEnumerable<Creature> enemies = MoveUser.CombatState.HittableEnemies;
        if (validTargetPredicate is { } isValidTarget)
            enemies = enemies.Where(c => isValidTarget(c));
        return MoveUser.RunRng.CombatTargets.NextItem(enemies);
    }
}

public class MoveContext<T>(T moveUser, Player petOwner, MoveParams moveParams)
    : MoveContext(petOwner, moveParams) where T : Figment
{
    public override T MoveUser { get; } = moveUser;
}