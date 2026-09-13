using Trimaw.Core.Utils;

namespace Trimaw.Core.SnackSystem;

public interface IMorselEnchanter : IEnchanter
{
    public Morsel Morsel { get; }
}