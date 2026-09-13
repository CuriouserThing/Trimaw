namespace Trimaw.Core.ImaginationSystem;

public class FigmentSlotMap(params FigmentSlot[] slots)
{
    public IReadOnlyList<FigmentSlot> Slots => slots;
}