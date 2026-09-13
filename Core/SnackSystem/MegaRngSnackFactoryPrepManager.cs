using MegaCrit.Sts2.Core.Random;

namespace Trimaw.Core.SnackSystem;

public class MegaRngSnackFactoryPrepManager(ISnackFactory snackFactory, Rng rng) : IPrepManager
{
    private readonly Morsel[] _oldPrep = new Morsel[SnackConstants.PrepSlots];
    private readonly Morsel[] _prep = new Morsel[SnackConstants.PrepSlots];
    private int _morselCount;
    private int _oldMorselCount;
    private EncodedSnack? _recentSnack;

    public void ResetPrepState()
    {
        _morselCount = 0;
        _oldMorselCount = 0;
        _recentSnack = null;
    }

    public IReadOnlyList<Morsel> CurrentPrep => _prep[.._morselCount];

    public Morsel? PreviousMorsel
    {
        get
        {
            if (_morselCount > 0)
                return _prep[_morselCount - 1];
            if (_oldMorselCount > 0)
                return _oldPrep[_oldMorselCount - 1];
            return null;
        }
    }

    public SnackResult? AddMorsel(Morsel morsel)
    {
        if (_morselCount < _prep.Length)
        {
            _prep[_morselCount] = morsel;
            _morselCount += 1;
        }

        if (_morselCount < _prep.Length) return null;

        Array.Copy(_prep, _oldPrep, _morselCount);
        _oldMorselCount = _morselCount;

        var morsels = _prep.AsSpan();
        var result = snackFactory.PullFromStock(morsels);
        _recentSnack = result.CreatedSnack;

        var c = 0;
        for (var i = 0; i < morsels.Length; i += 1)
            if (!result.MorselIsUsedInSnack(i))
            {
                morsels[c] = morsels[i];
                c += 1;
            }

        _morselCount = c;
        return result;
    }

    public Morsel GenerateRandomMorsel()
    {
        var roll = rng.NextInt(SnackConstants.MorselKinds);
        return (Morsel)roll;
    }

    public static MegaRngSnackFactoryPrepManager FromRandomSeed(ISnackFactory snackFactory)
    {
        return new MegaRngSnackFactoryPrepManager(snackFactory, Rng.Chaotic);
    }

    public static MegaRngSnackFactoryPrepManager FromSeed(ISnackFactory snackFactory, uint seed)
    {
        var rng = new Rng(seed);
        return new MegaRngSnackFactoryPrepManager(snackFactory, rng);
    }

    public ReadOnlySpan<Morsel> GetMorsels()
    {
        return _prep.AsSpan(.._morselCount);
    }
}