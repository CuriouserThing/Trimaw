namespace Trimaw.Core.SnackSystem;

public interface ISnackFactory
{
    public SnackResult PullFromStock(ReadOnlySpan<Morsel> stock);
}