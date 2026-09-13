namespace Trimaw.Core.ImaginationSystem;

/// <summary>
///     Mod-dependent figment tags with some ambiguity (see <see cref="HardTag" />)
/// </summary>
public enum SoftTag : ushort
{
    None = 0,
    CommonRarity,
    UncommonRarity,
    ExcludedFromGacha,
    ExcludedFromAllRolls
}