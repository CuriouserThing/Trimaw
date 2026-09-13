using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using Trimaw.Core.Models.Monsters;

namespace Trimaw.Core.Models.Powers;

/// <summary>
///     Umbrella for all powers that figments apply to themselves on creation or afterward.
/// </summary>
public abstract class FigmentPower : TrimawPower
{
    public override PowerType Type => PowerType.Buff;

    protected Figment Figment =>
        Owner.Monster as Figment
        ?? throw new InvalidOperationException($"Only apply {GetType()} power to a {typeof(Figment)} monster.");

    /// <summary>
    ///     For simplicity and expedience in <see cref="Figment.Pop" />, regulate access to this hook.
    /// </summary>
    public sealed override async Task AfterRemoved(Creature oldOwner)
    {
        if (oldOwner.IsAlive && Figment.IsAvailable) await AfterRemoved();
    }

    protected virtual Task AfterRemoved()
    {
        return Task.CompletedTask;
    }
}