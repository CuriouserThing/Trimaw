using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Models.Enchantments;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class DailyDoodlesPower : TrimawPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Doodled>();

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != CombatSide.Player || Owner.Player is not { } player) return;

        var doodled = ModelDb.Enchantment<Doodled>();

        var candidates = PileType.Hand.GetPile(player).Cards
            .Where(c => c.Enchantment is null && doodled.CanEnchant(c))
            .ToArray();
        var amountToTransform = Math.Min(Amount, candidates.Length);
        if (amountToTransform > 0)
        {
            var cardsToTransform = candidates
                .TakeRandom(amountToTransform, player.RunState.Rng.CombatCardSelection)
                .ToArray();
            var transformedCards = new CardModel[amountToTransform];
            for (var i = 0; i < amountToTransform; i += 1)
            {
                var clone = cardsToTransform[i].CreateClone();
                CardCmd.Enchant<Doodled>(clone, 1);
                transformedCards[i] = clone;
            }

            var transforms = cardsToTransform.Zip(transformedCards, (a, b) => new CardTransformation(a, b));
            await CardCmd.Transform(transforms, null);
        }

        var amountToGenerate = Amount - amountToTransform;
        if (amountToGenerate > 0)
        {
            var generatedCards = CardFactory.GetDistinctForCombat(
                player,
                player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
                    .Where(doodled.CanEnchant),
                amountToGenerate,
                player.RunState.Rng.CombatCardGeneration).ToArray();
            foreach (var card in generatedCards)
                CardCmd.Enchant<Doodled>(card, 1);
            await CardPileCmd.AddGeneratedCardsToCombat(generatedCards, PileType.Hand, player);
        }
    }
}