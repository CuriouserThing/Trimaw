using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Trimaw.Core.Models.Cards;

[Pool(typeof(TokenCardPool))]
public abstract class TrimawTokenCard(
    int cost,
    CardType type,
    CardRarity rarity,
    TargetType target,
    bool showInCardLibrary = true) :
    BaseTrimawCard(cost, type, rarity, target, showInCardLibrary)
{
}