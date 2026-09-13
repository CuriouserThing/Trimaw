using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.Models.Monsters;

/// <summary>
///     <see cref="SimpleFigment{TTalent,TIntent}" /> with placeholder talent & intent, and a Defend's worth of HP.
/// </summary>
public abstract class PlaceholderFigment : SimpleFigment<PlaceholderTalent, PlaceholderIntent>
{
    protected override int InitialHp => 5;
}