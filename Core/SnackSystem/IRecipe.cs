using System.Diagnostics.CodeAnalysis;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using Trimaw.Core.Models.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.SnackSystem;

public interface IRecipe
{
    public string Name { get; }
    public bool IsValid { get; }
    public IReadOnlyList<Morsel> SnackBase { get; }
    public IReadOnlyList<Morsel> Upgraders { get; }
    public IReadOnlyDictionary<Morsel, IMorselEnchanter> Enchanters { get; }
    public bool IsPreUpgraded();
    public bool IsPreEnchanted([NotNullWhen(true)] out IEnchanter? enchanter);
    public SnackCard CreateCard(ICombatState combatState, Player owner);
}