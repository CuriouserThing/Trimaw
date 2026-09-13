using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using Trimaw.Core.Models.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.SnackSystem;

internal abstract class BaseRecipe<T>(Morsel[] snackBase) : IRecipe where T : SnackCard
{
    public virtual IReadOnlyList<Morsel> EnchanterMorsels => [];
    public string Name => typeof(T).Name;
    public bool IsValid { get; private set; }
    public IReadOnlyList<Morsel> SnackBase { get; } = snackBase;
    public virtual IReadOnlyList<Morsel> Upgraders => [];

    public virtual IReadOnlyDictionary<Morsel, IMorselEnchanter> Enchanters =>
        ImmutableDictionary<Morsel, IMorselEnchanter>.Empty;

    public virtual bool IsPreUpgraded()
    {
        return false;
    }

    public virtual bool IsPreEnchanted([NotNullWhen(true)] out IEnchanter? enchanter)
    {
        enchanter = null;
        return false;
    }

    public SnackCard CreateCard(ICombatState combatState, Player owner)
    {
        return combatState.CreateCard<T>(owner);
    }

    protected void Validate(int baseSize, IMorselEnchanter[] enchanters, Morsel[] snackBase, Morsel[] upgraders)
    {
        Span<bool> flags = stackalloc bool[SnackConstants.MorselKinds];

        if (snackBase.Length != baseSize) return;

        foreach (var upgrader in upgraders)
        {
            if (flags[(int)upgrader]) return;

            flags[(int)upgrader] = true;
        }

        foreach (var enchanter in enchanters)
        {
            var morsel = enchanter.Morsel;
            if (flags[(int)morsel]) return;

            flags[(int)morsel] = true;
        }

        IsValid = true;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        foreach (var morsel in EnchanterMorsels)
        {
            var e = Printing.Emojify(morsel);
            var name = Enchanters[morsel].Name;
            builder.Append($"{{{e} {name}}} ");
        }

        if (SnackBase.Count > 0)
        {
            var b = Printing.Emojify(SnackBase);
            builder.Append($"{b} ");
        }

        if (IsPreEnchanted(out var enchanter)) builder.Append($"{{{enchanter.Name}}} ");

        builder.Append($"{Name}");
        if (IsPreUpgraded()) builder.Append('+');

        foreach (var upgrader in Upgraders)
        {
            var u = Printing.Emojify(upgrader);
            builder.Append($" ({u}+)");
        }

        return builder.ToString();
    }
}