using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.SnackSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Rare;

public class Hope() : TrimawCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        HoverTipHelper.ForImagine(this).Concat(HoverTipHelper.ForMorselPrep(Morsel.Bread));

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ImaginationCmd.ImagineRandom<ImaginaryFriendPower>(choiceContext, Owner);

        await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Bread, Owner);
        if (IsUpgraded)
            await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Water, Owner);
    }
}