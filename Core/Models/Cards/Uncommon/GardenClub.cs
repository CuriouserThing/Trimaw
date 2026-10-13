using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using Trimaw.Core.Commands;
using Trimaw.Core.SnackSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class GardenClub() : TrimawCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipHelper.ForManifest<Sown>()
        .Concat(HoverTipHelper.ForMorselPrep(Morsel.Pepper, Morsel.Berries));

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await EnchantCmd.Manifest(choiceContext, Owner, PileType.Hand, new Enchanter<Sown>(),
            DynamicVars.Cards.IntValue);

        await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Pepper, Owner);
        await SnackCmd.PrepSpecific(choiceContext, CombatState, Morsel.Berries, Owner);
    }
}