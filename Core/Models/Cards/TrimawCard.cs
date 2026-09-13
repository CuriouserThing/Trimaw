using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Trimaw.Core.Models.Cards;

[Pool(typeof(TrimawCardPool))]
public abstract class TrimawCard(
    int cost,
    CardType type,
    CardRarity rarity,
    TargetType target,
    bool showInCardLibrary = true) :
    BaseTrimawCard(cost, type, rarity, target, showInCardLibrary)
{
}