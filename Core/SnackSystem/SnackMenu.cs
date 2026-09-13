using System.Diagnostics.CodeAnalysis;

namespace Trimaw.Core.SnackSystem;

public class SnackMenu
{
    private readonly List<IRecipe> _invalidRecipes = [];
    private readonly HashSet<EncodedSnack> _repeatedSnacks = [];
    private readonly List<IReadOnlyList<Morsel>> _snacklessStocks = [];
    private readonly Dictionary<ulong, EncodedSnack> _snacks = [];

    public SnackMenu(IEnumerable<IRecipe> recipes)
    {
        foreach (var recipe in recipes)
        {
            if (!recipe.IsValid)
            {
                _invalidRecipes.Add(recipe);
                continue;
            }

            foreach (var snack in EncodedSnack.AllFromRecipe(recipe))
                if (!_snacks.TryAdd(snack.Code, snack))
                    _ = _repeatedSnacks.Add(snack);
        }

        foreach (var stock in GetStockCombinations())
            if (GetMatchCount(stock) == 0)
                _snacklessStocks.Add(stock);
    }

    public IReadOnlyDictionary<ulong, EncodedSnack> Snacks => _snacks;

    public IReadOnlyList<IRecipe> InvalidRecipes => _invalidRecipes;

    public IReadOnlySet<EncodedSnack> RepeatedSnacks => _repeatedSnacks;

    public IReadOnlyList<IReadOnlyList<Morsel>> SnacklessStocks => _snacklessStocks;

    public bool IsValid => _invalidRecipes.Count == 0 && _repeatedSnacks.Count == 0 && _snacklessStocks.Count == 0;

    internal int GetMatchCount(ReadOnlySpan<Morsel> stock)
    {
        var count = 0;
        Span<Morsel> stockBuffer = stackalloc Morsel[SnackConstants.PrepSlots];
        var permutationCount = 1 << stock.Length;
        for (var p = 0; p < permutationCount; p += 1)
        {
            stock.CopyTo(stockBuffer);
            var code = EncodedSnack.Encode(Filter(stockBuffer, p));

            if (_snacks.ContainsKey(code)) count += 1;
        }

        return count;
    }

    internal bool TryPullFirstFromStock(ReadOnlySpan<Morsel> stock, [NotNullWhen(true)] out SnackResult? result)
    {
        result = null;
        Span<Morsel> stockBuffer = stackalloc Morsel[SnackConstants.PrepSlots];
        var permutationCount = 1 << stock.Length;
        for (var p = 0; p < permutationCount; p += 1)
        {
            var bitfield = permutationCount - p - 1; // full bitfield first, then descend
            stock.CopyTo(stockBuffer);
            var code = EncodedSnack.Encode(Filter(stockBuffer, bitfield));

            if (!_snacks.TryGetValue(code, out var snack)) continue;

            result = new SnackResult(snack, bitfield);
            return true;
        }

        return false;
    }

    private static Span<Morsel> Filter(Span<Morsel> morsels, int bitfield)
    {
        var tail = 0;
        for (var i = 0; i < morsels.Length; i += 1)
        {
            var bit = bitfield & (1 << i);
            if (bit != 0)
            {
                morsels[tail] = morsels[i];
                tail += 1;
            }
        }

        return morsels[..tail];
    }

    private static List<Morsel[]> GetStockCombinations()
    {
        var palette = Enumerable.Range(0, SnackConstants.MorselKinds).Select(i => (Morsel)i).ToArray();
        Span<Morsel> slots = stackalloc Morsel[SnackConstants.PrepSlots];
        var combinations = new List<Morsel[]>();
        Combinate(palette, slots, combinations);
        return combinations;
    }

    private static void Combinate(ReadOnlySpan<Morsel> palette, Span<Morsel> slots, List<Morsel[]> output,
        int depth = 0)
    {
        if (depth == SnackConstants.PrepSlots)
        {
            var morsels = new Morsel[depth];
            slots.CopyTo(morsels);
            output.Add(morsels);
            return;
        }

        for (var i = 0; i < palette.Length; i += 1)
        {
            slots[depth] = palette[i];
            Combinate(palette[i..], slots, output, depth + 1);
        }
    }
}