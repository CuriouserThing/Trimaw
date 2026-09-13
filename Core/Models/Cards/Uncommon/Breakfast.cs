using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Trimaw.Core.Commands;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class Breakfast() : TrimawCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Prep),
        HoverTipFactory.Static(StaticHoverTip.Bread),
        HoverTipFactory.Static(StaticHoverTip.Berries)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate, CardKeyword.Exhaust];

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Bread, Owner);
        await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Berries, Owner);
        await SnackCmd.PrepRandom(choiceContext, CombatState, Owner);
    }
}