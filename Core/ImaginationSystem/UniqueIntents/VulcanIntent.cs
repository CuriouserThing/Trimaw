using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class VulcanIntent : UnlabeledFigmentIntent<VulcanFigment>
{
    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Buff;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("hammer");

    private static IEnumerable<CardModel> GetCards(MoveContext ctx)
    {
        return PileType.Hand.GetPile(ctx.PetOwner).Cards.Where(c => c.IsUpgradable);
    }

    protected override bool CanPerform(MoveContext<VulcanFigment> ctx, out Creature? target)
    {
        target = ctx.PetOwner.Creature;
        return ctx.PetOwner.Creature.IsAlive && GetCards(ctx).Any();
    }

    protected override Task<FigmentMoveResult> OnPerform(MoveContext<VulcanFigment> ctx, PlayerChoiceContext choiceCtx)
    {
        var cards = PileType.Hand.GetPile(ctx.PetOwner).Cards.Where(c => c.IsUpgradable).ToArray();
        if (cards.Length == 0) return Task.FromResult(FigmentMoveResult.NoValidTarget);
        foreach (var card in cards) CardCmd.Upgrade(card);
        return Task.FromResult(FigmentMoveResult.Success);
    }
}