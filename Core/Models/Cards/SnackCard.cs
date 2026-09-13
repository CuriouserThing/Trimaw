using MegaCrit.Sts2.Core.Entities.Cards;

namespace Trimaw.Core.Models.Cards;

public abstract class SnackCard(bool showInCardLibrary = true)
    : TrimawTokenCard(0, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public sealed override IEnumerable<CardKeyword> CanonicalKeywords =>
        ExtraCanonicalKeywords.Append(CardKeyword.Exhaust);

    protected virtual IEnumerable<CardKeyword> ExtraCanonicalKeywords => [];
}