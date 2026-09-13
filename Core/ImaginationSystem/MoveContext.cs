using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.ImaginationSystem;

public abstract class MoveContext(Player petOwner, MoveParams moveParams)
{
    public TriggerKind Trigger { get; init; } = TriggerKind.Unknown;

    public abstract Figment MoveUser { get; }

    public Player PetOwner { get; } = petOwner;

    public MoveParams Params { get; } = moveParams;

    public ICombatState CombatState => MoveUser.CombatState;

    public MegaAnimationState? AnimationState =>
        MoveUser.Creature.GetCreatureNode()?.Visuals.SpineBody?.TryGetAnimationState();

    /// <summary>
    ///     Non-negative numeric param for the move to use. As with <see cref="Creature.GetPowerAmount" />, 0 implies no
    ///     particular amount (the caller must pass only positive amounts).
    /// </summary>
    /// <param name="allowEstimate">
    ///     Whether to request an estimate if the caller passed no amount (for intent labels, tip
    ///     descriptions, etc.).
    /// </param>
    public decimal GetAmount(bool allowEstimate = false)
    {
        if (Params.Amount is { } amount and > 0) return amount;
        if (!allowEstimate) return 0;

        return MoveUser.Creature.Powers
            .Select(p => Math.Max(0, (p as TriggerTalent)?.EstimatedAmount ?? 0))
            .DefaultIfEmpty(0)
            .Max();
    }

    /// <summary>
    ///     Creature for the move to target. A non-null target may come from either the caller or a random selection. Null
    ///     implies that no valid target is available.
    /// </summary>
    /// <param name="validTargetPredicate">
    ///     Optional predicate to narrow down any random selection from the set of all hittable
    ///     enemies.
    /// </param>
    public Creature? GetTarget(Predicate<Creature>? validTargetPredicate = null)
    {
        if (Params.Target is { IsAlive: true } target) return target;

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