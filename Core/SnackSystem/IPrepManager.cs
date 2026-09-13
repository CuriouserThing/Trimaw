namespace Trimaw.Core.SnackSystem;

public interface IPrepManager
{
    IReadOnlyList<Morsel> CurrentPrep { get; }

    Morsel? PreviousMorsel { get; }

    void ResetPrepState();

    SnackResult? AddMorsel(Morsel morsel);

    Morsel GenerateRandomMorsel();
}