namespace Trimaw.Core.SnackSystem;

public class SnackResult
{
    private readonly int _slotsUsedBitfield;

    public SnackResult(EncodedSnack createdSnack, int slotsUsedBitfield)
    {
        CreatedSnack = createdSnack;
        _slotsUsedBitfield = slotsUsedBitfield;
    }

    public EncodedSnack CreatedSnack { get; }

    /// <summary>
    ///     Is the morsel at this index used in <see cref="CreatedSnack" />?
    /// </summary>
    /// <param name="idx">0 indexes the oldest (most-significant) morsel.</param>
    public bool MorselIsUsedInSnack(int idx)
    {
        var shift = SnackConstants.PrepSlots - 1 - idx;
        return ((_slotsUsedBitfield >> shift) & 1) == 1;
    }
}