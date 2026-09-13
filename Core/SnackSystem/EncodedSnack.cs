using System.Diagnostics.CodeAnalysis;
using System.Text;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using Trimaw.Core.Models.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.SnackSystem;

/// <summary>
///     A recipe turned into a snack by (optionally) picking an upgrader from the recipe and (optionally) picking an
///     enchanter from it.
///     Its code represents all morsels in the snack, without regard for order or whether each morsel is from the base,
///     upgrader, or enchanter.
/// </summary>
/// <remarks>
///     The code is not unique to the recipe! Use only for detecting morsel collisions.
/// </remarks>
public class EncodedSnack
{
    private readonly Morsel? _upgrader, _enchanterMorsel;

    private EncodedSnack(ulong code, IRecipe recipe, Morsel? upgrader = null, Morsel? enchanterMorsel = null)
    {
        Code = code;
        Recipe = recipe;
        _upgrader = upgrader;
        _enchanterMorsel = enchanterMorsel;
    }

    public ulong Code { get; }

    public IRecipe Recipe { get; }

    public bool IsUpgraded => Recipe.IsPreUpgraded() || _upgrader is not null;

    public bool IsEnchanted => Recipe.IsPreEnchanted(out _) || _enchanterMorsel is not null;

    public int MorselCount => SnackConstants.SnackBaseSize + (IsUpgraded ? 1 : 0) + (IsEnchanted ? 1 : 0);

    public Morsel? MaybeUpgrader => _upgrader;

    public IEnchanter? MaybeEnchanter
    {
        get
        {
            _ = TryGetEnchanter(out var enchanter);
            return enchanter;
        }
    }

    public IMorselEnchanter? MaybeMorselEnchanter
    {
        get
        {
            if (_enchanterMorsel.HasValue)
                return Recipe.Enchanters[_enchanterMorsel.Value];
            return null;
        }
    }

    public bool TryGetEnchanter([NotNullWhen(true)] out IEnchanter? enchanter)
    {
        if (Recipe.IsPreEnchanted(out enchanter)) return true;

        if (_enchanterMorsel.HasValue)
        {
            var result = Recipe.Enchanters.TryGetValue(_enchanterMorsel.Value, out var e);
            enchanter = e;
            return result;
        }

        enchanter = null;
        return false;
    }

    public SnackCard ToCard(ICombatState combatState, Player owner)
    {
        var card = Recipe.CreateCard(combatState, owner);
        if (IsUpgraded) CardCmd.Upgrade(card);

        if (TryGetEnchanter(out var enchanter))
        {
            var enchantment = enchanter.Enchant(card);
            if (enchantment is null)
            {
                // TODO: warn?
            }
        }

        return card;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        if (_enchanterMorsel.HasValue) builder.Append(Printing.Emojify(_enchanterMorsel.Value));

        builder.Append(Printing.Emojify(Recipe.SnackBase));

        if (_upgrader.HasValue) builder.Append(Printing.Emojify(_upgrader.Value));

        builder.Append(' ');

        if (TryGetEnchanter(out var enchanter)) builder.Append($"{{{enchanter.Name}}} ");

        builder.Append(Recipe.Name);
        if (IsUpgraded) builder.Append('+');

        return builder.ToString();
    }

    #region Encoding

    private static readonly ulong[] EncodingTable;

    static EncodedSnack()
    {
        ulong currentPower = 1;
        const int numericBase = SnackConstants.PrepSlots + 1;
        EncodingTable = new ulong[SnackConstants.MorselKinds];
        for (var i = 0; i < SnackConstants.MorselKinds; i += 1)
        {
            EncodingTable[i] = currentPower;
            currentPower *= numericBase;
        }
    }

    private static ulong Encode(ulong seed, Morsel morsel)
    {
        return seed + EncodingTable[(int)morsel];
    }

    private static ulong Encode(ulong seed, ReadOnlySpan<Morsel> morsels)
    {
        var code = seed;
        foreach (var morsel in morsels) code = Encode(code, morsel);

        return code;
    }

    internal static ulong Encode(ReadOnlySpan<Morsel> morsels)
    {
        return Encode(0, morsels);
    }

    private class FallbackRecipe<T>(Morsel[] snackBase) : BaseRecipe<T>(snackBase) where T : SnackCard;

    internal static EncodedSnack FromMorsels<T>(Morsel[] morsels) where T : SnackCard
    {
        var code = Encode(morsels);
        var recipe = new FallbackRecipe<T>(morsels);
        return new EncodedSnack(code, recipe);
    }

    internal static IReadOnlyList<EncodedSnack> AllFromRecipe(IRecipe recipe)
    {
        var list = new List<EncodedSnack>();

        var code = recipe.SnackBase.Aggregate(0UL, Encode);
        list.Add(new EncodedSnack(code, recipe));

        foreach (var morsel in recipe.Enchanters.Keys)
        {
            var eCode = Encode(code, morsel);
            list.Add(new EncodedSnack(eCode, recipe, null, morsel));
        }

        foreach (var upgrader in recipe.Upgraders)
        {
            var uCode = Encode(code, upgrader);
            list.Add(new EncodedSnack(uCode, recipe, upgrader));

            foreach (var morsel in recipe.Enchanters.Keys)
            {
                var ueCode = Encode(uCode, morsel);
                list.Add(new EncodedSnack(ueCode, recipe, upgrader, morsel));
            }
        }

        return list;
    }

    #endregion
}