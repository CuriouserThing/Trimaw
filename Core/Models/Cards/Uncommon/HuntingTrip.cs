using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Models.Powers.CeobePowers;
using Trimaw.Core.SnackSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class HuntingTrip() : TrimawCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new[] { HoverTipFactory.FromPower<AimPower>() }.Concat(HoverTipHelper.ForMorselPrep(Morsel.Meat));

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<AimPower>(6)];

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(AimPower)].UpgradeValueBy(6);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<AimPower>(choiceContext, Owner.Creature, DynamicVars[nameof(AimPower)].BaseValue,
            Owner.Creature, this);
        await PowerCmd.Apply<MeatNextTurnPower>(choiceContext, Owner.Creature, DynamicVars[nameof(AimPower)].BaseValue,
            Owner.Creature, this);
    }
}