using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Monsters;

namespace Trimaw.Core.ImaginationSystem;

public abstract class TaggedFigment(IReadOnlySet<FigmentFilter.Tag> tags)
{
    public abstract Type FigmentType { get; }

    public IReadOnlySet<FigmentFilter.Tag> Tags { get; } = tags;

    public abstract Task Imagine(PlayerChoiceContext choiceContext, Player owner, FigmentSlotMap slotMap);

    public static TaggedFigment FromModel<T>() where T : Figment
    {
        var model = ModelDb.Monster<T>();
        var tags = model.HardTags.Select(t => (FigmentFilter.Tag)t).ToHashSet();
        return new TaggedFigment<T>(tags);
    }
}

internal class TaggedFigment<T>(IReadOnlySet<FigmentFilter.Tag> tags) : TaggedFigment(tags)
    where T : Figment
{
    public override Type FigmentType => typeof(T);

    public override async Task Imagine(PlayerChoiceContext choiceContext, Player owner, FigmentSlotMap slotMap)
    {
        await ImaginationCmd.Imagine<T>(choiceContext, owner, slotMap);
    }
}