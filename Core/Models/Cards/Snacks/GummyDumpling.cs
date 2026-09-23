using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Powers.Triggers;

namespace Trimaw.Core.Models.Cards.Snacks;

/// <remarks>
///     Based on pelmeni. Gummy actually makes this at one point!
/// </remarks>
public class GummyDumpling : SnackCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BearNecessitiesTrigger>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ImaginationCmd.ImagineRandom<BearNecessitiesTrigger>(choiceContext, Owner);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
}