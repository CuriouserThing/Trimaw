using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Commands;

namespace Trimaw.Core.Models.Cards.Common;

public class CookbookClipping() : TrimawCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    private const string MorselsKey = "Morsels";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Prep)];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(MorselsKey, 1),
        new CardsVar(1)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars[MorselsKey].UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);

        int morsels = DynamicVars[MorselsKey].IntValue;
        for (int i = 0; i < morsels; i += 1)
        {
            await SnackCmd.PrepRandom(choiceContext, CombatState, Owner);
        }
    }
}