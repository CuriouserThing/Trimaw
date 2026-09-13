using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using Trimaw.Core.Utils;

namespace Trimaw.Core;

public class MegaRngEnchantmentPoolSuperSpecializer(IReadOnlyList<IEnchanter> enchantmentPool, Rng rng)
    : ISuperSpecializer
{
    public void MakeSuperSpecial(CardModel card)
    {
        if (card.Enchantment is not null)
        {
            MainFile.Logger.Warn($"{card} is already enchanted so cannot be made Super Special.");
            return;
        }

        if (enchantmentPool
                .Where(e => e.CanEnchant(card))
                .TakeRandom(1, rng)
                .FirstOrDefault() is not { } enchanter)
        {
            MainFile.Logger.Error($"No Super Special enchantment is valid on {card}.");
            return;
        }

        MainFile.Logger.Info($"Randomly selected Super Special enchantment {enchanter} for card.");
        CardCmd.ClearEnchantment(card);

        if (enchanter.Enchant(card) is null)
            MainFile.Logger.Error($"Apparently could not enchant {card} with {enchanter}.");
    }
}