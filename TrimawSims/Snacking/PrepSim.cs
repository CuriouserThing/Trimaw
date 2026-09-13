using Trimaw.Core.SnackSystem;

namespace TrimawSims.Snacking;

internal class PrepSim
{
    private readonly MegaRngSnackFactoryPrepManager _manager;
    private readonly IReadOnlyDictionary<Morsel, int> _morselWeights;
    private readonly Random _rand;
    private readonly IReadOnlyDictionary<Selection, int> _selectionWeights;
    private readonly int _totalMorselWeight;
    private readonly int _totalSelectionWeight;

    public PrepSim(MegaRngSnackFactoryPrepManager manager, Random rand,
        IReadOnlyDictionary<Selection, int> selectionWeights,
        IReadOnlyDictionary<Morsel, int> morselWeights)
    {
        _manager = manager;
        _rand = rand;
        _selectionWeights = selectionWeights;
        _totalSelectionWeight = selectionWeights.Sum(kvp => kvp.Value);
        _morselWeights = morselWeights;
        _totalMorselWeight = morselWeights.Sum(kvp => kvp.Value);
    }

    public EncodedSnack? Next()
    {
        var selection = Next(_selectionWeights, _totalSelectionWeight);
        if (selection == Selection.ClearAllMorsels)
        {
            _manager.ResetPrepState();
            return null;
        }

        var morsel = selection switch
        {
            Selection.PrepRandom => _manager.GenerateRandomMorsel(),
            Selection.PrepWeighted => Next(_morselWeights, _totalMorselWeight),
            Selection.PrepLast => _manager.PreviousMorsel ?? _manager.GenerateRandomMorsel(),
            _ => throw new Exception("Invalid selection")
        };
        return _manager.AddMorsel(morsel)?.CreatedSnack;
    }

    public ReadOnlySpan<Morsel> GetMorsels()
    {
        return _manager.GetMorsels();
    }

    private T Next<T>(IReadOnlyDictionary<T, int> weights, int totalWeight)
    {
        var x = _rand.Next(0, totalWeight);
        var n = 0;
        foreach (var (item, weight) in weights)
        {
            n += weight;
            if (x < n) return item;
        }

        throw new Exception("Weighted random failure");
    }
}