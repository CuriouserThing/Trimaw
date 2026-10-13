using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Enchantments;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Snacks;

public class BoxOfTruffles : SnackCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        HoverTipHelper.ForImagineRandom<ArtsAssimilationTalent>(this)
            .Concat(HoverTipHelper.ForManifest<ReallyCold>());

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3),
        new(nameof(ReallyCold), 3)
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ImaginationCmd.ImagineRandom<ArtsAssimilationTalent>(choiceContext, Owner);

        var enchanter = new Enchanter<ReallyCold>(DynamicVars[nameof(ReallyCold)].IntValue);
        await EnchantCmd.Manifest(choiceContext, Owner, PileType.Draw, enchanter, DynamicVars.Cards.IntValue);
    }
}