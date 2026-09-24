using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Extensions;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class ReallyWet : ReallyXEnchantment
{
    public override string Icon64Path => Pathfinder.NotoEmoji64("splashing_sweat_symbol");

    protected override async Task Operate()
    {
        var choiceCtx = GetChoiceContext();
        var cards = (await CardPileCmd.Draw(choiceCtx, Amount, Card.Owner)).ToArray();
        if (cards.Length < 2) return;

        var cardsToDiscard = cards.TakeRandom(cards.Length - 1, Card.Owner.RunState.Rng.CombatTargets);
        await CardCmd.Discard(choiceCtx, cardsToDiscard);
    }
}