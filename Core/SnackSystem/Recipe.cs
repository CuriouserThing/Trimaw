using System.Diagnostics.CodeAnalysis;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Models.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.SnackSystem;

internal class Recipe<T> : BaseRecipe<T> where T : SnackCard
{
    private readonly Morsel[] _enchanterMorsels;
    private readonly Dictionary<Morsel, IMorselEnchanter> _enchanters;
    private readonly Morsel[] _upgraders;

    public Recipe(IMorselEnchanter[] enchanters, Morsel[] snackBase, params Morsel[] upgraders) : base(snackBase)
    {
        const int baseSize = SnackConstants.SnackBaseSize;
        Validate(baseSize, enchanters, snackBase, upgraders);
        if (!IsValid)
        {
            _upgraders = [];
            _enchanterMorsels = [];
            _enchanters = [];
            return;
        }

        _upgraders = upgraders;
        _enchanterMorsels = new Morsel[enchanters.Length];
        _enchanters = new Dictionary<Morsel, IMorselEnchanter>(enchanters.Length);
        for (var i = 0; i < enchanters.Length; i += 1)
        {
            var enchanter = enchanters[i];
            _enchanterMorsels[i] = enchanter.Morsel;
            _enchanters.Add(enchanter.Morsel, enchanter);
        }
    }

    public Recipe(Morsel[] snackBase, params Morsel[] upgraders) : this([], snackBase, upgraders)
    {
    }

    public override IReadOnlyList<Morsel> Upgraders => _upgraders;

    public override IReadOnlyList<Morsel> EnchanterMorsels => _enchanterMorsels;

    public override IReadOnlyDictionary<Morsel, IMorselEnchanter> Enchanters => _enchanters;
}

internal class Recipe<TEnchant, TRecipe> : BaseRecipe<TRecipe>
    where TEnchant : EnchantmentModel where TRecipe : SnackCard
{
    private readonly IEnchanter _enchanter;
    private readonly Morsel[] _upgraders;

    private Recipe(Enchanter<TEnchant> enchanter, Morsel[] snackBase, Morsel[] upgraders) : base(snackBase)
    {
        _enchanter = enchanter;

        const int baseSize = SnackConstants.SnackBaseSize + 1;
        Validate(baseSize, [], snackBase, upgraders);
        if (!IsValid)
        {
            _upgraders = [];
            return;
        }

        _upgraders = upgraders;
    }

    public Recipe(Morsel[] snackBase, params Morsel[] upgraders)
        : this(new Enchanter<TEnchant>(), snackBase, upgraders)
    {
    }

    public Recipe(int enchantAmount, Morsel[] snackBase, params Morsel[] upgraders)
        : this(new Enchanter<TEnchant>(enchantAmount), snackBase, upgraders)
    {
    }

    public override IReadOnlyList<Morsel> Upgraders => _upgraders;

    public override bool IsPreEnchanted([NotNullWhen(true)] out IEnchanter? enchanter)
    {
        enchanter = _enchanter;
        return true;
    }
}