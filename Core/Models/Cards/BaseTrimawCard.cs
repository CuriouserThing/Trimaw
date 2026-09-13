using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards;

public abstract class BaseTrimawCard(
    int cost,
    CardType type,
    CardRarity rarity,
    TargetType target,
    bool showInCardLibrary = true) :
    CustomCardModel(cost, type, rarity, target, showInCardLibrary)
{
    public override string PortraitPath => Pathfinder.CardPortrait(this);
    public override string BetaPortraitPath => Pathfinder.BetaCardPortrait(this);
}