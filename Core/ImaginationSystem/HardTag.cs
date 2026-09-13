namespace Trimaw.Core.ImaginationSystem;

/// <summary>
///     Immutable figment tags with no ambiguity (see <see cref="SoftTag" />)
/// </summary>
public enum HardTag : ushort
{
    None = 0,

    // Function
    Attacking,

    // Original IP
    FromArknights,
    FromDungeonMeshi,
    FromSlayTheSpire,

    // Sprite source
    UsesAkChar,
    UsesAkEnemy,
    UsesSts2Player,
    UsesSts2Monster,

    // Hard Arknights facts
    Slug,
    ReserveOp,
    Tiacauh,
    UrsusRace,
    DuckLordAssociate
}