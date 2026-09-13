namespace Trimaw.Core.SnackSystem;

public class SnackFactory(SnackMenu menu, ISnackFactory fallback) : ISnackFactory
{
    public SnackMenu Menu { get; } = menu;

    public ISnackFactory Fallback { get; } = fallback;

    public bool UseFifoMorsels { get; init; } = false;

    public SnackResult PullFromStock(ReadOnlySpan<Morsel> stock)
    {
        Span<Morsel> buffer = stackalloc Morsel[SnackConstants.PrepSlots];
        stock.CopyTo(buffer);
        if (UseFifoMorsels) buffer.Reverse();

        // If this is true, it means at least one test has failed and a very simple bug has not yet been caught.
        // And yet... we handle it gracefully...
        if (!Menu.TryPullFirstFromStock(buffer, out var result) ||
            Menu.RepeatedSnacks.Contains(result.CreatedSnack))
            result = Fallback.PullFromStock(buffer);

        return result;
    }
}