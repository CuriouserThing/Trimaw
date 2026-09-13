using System.Diagnostics.CodeAnalysis;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Models.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.SnackSystem;

internal class UpgradedRecipe<T> : BaseRecipe<T> where T : SnackCard
{
    private readonly Morsel[] _enchanterMorsels;
    private readonly Dictionary<Morsel, IMorselEnchanter> _enchanters;

    public UpgradedRecipe(IMorselEnchanter[] enchanters, Morsel[] snackBase) : base(snackBase)
    {
        const int baseSize = SnackConstants.SnackBaseSize + 1;
        Validate(baseSize, enchanters, snackBase, []);
        if (!IsValid)
        {
            _enchanterMorsels = [];
            _enchanters = [];
            return;
        }

        _enchanterMorsels = new Morsel[enchanters.Length];
        _enchanters = new Dictionary<Morsel, IMorselEnchanter>(enchanters.Length);
        for (var i = 0; i < enchanters.Length; i += 1)
        {
            var enchanter = enchanters[i];
            _enchanterMorsels[i] = enchanter.Morsel;
            _enchanters.Add(enchanter.Morsel, enchanter);
        }
    }

    public UpgradedRecipe(Morsel[] snackBase) : this([], snackBase)
    {
    }

    public override IReadOnlyList<Morsel> EnchanterMorsels => _enchanterMorsels;

    public override IReadOnlyDictionary<Morsel, IMorselEnchanter> Enchanters => _enchanters;

    public override bool IsPreUpgraded()
    {
        return true;
    }
}

internal class UpgradedRecipe<TEnchant, TRecipe> : BaseRecipe<TRecipe>
    where TEnchant : EnchantmentModel where TRecipe : SnackCard
{
    private readonly IEnchanter _enchanter;

    private UpgradedRecipe(Enchanter<TEnchant> enchanter, Morsel[] snackBase) : base(snackBase)
    {
        _enchanter = enchanter;

        const int baseSize = SnackConstants.SnackBaseSize + 2;
        Validate(baseSize, [], snackBase, []);
    }

    public UpgradedRecipe(Morsel[] snackBase)
        : this(new Enchanter<TEnchant>(), snackBase)
    {
    }

    public UpgradedRecipe(int enchantAmount, Morsel[] snackBase)
        : this(new Enchanter<TEnchant>(enchantAmount), snackBase)
    {
    }

    public override bool IsPreUpgraded()
    {
        return true;
    }

    public override bool IsPreEnchanted([NotNullWhen(true)] out IEnchanter? enchanter)
    {
        enchanter = _enchanter;
        return true;
    }
}