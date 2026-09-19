using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Models.Powers.CeobePowers;

namespace Trimaw.Core.Models.Cards.Rare;

public class Bifocal() : TrimawCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<AimPower>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<AimPower>(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<BifocalPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);

        if (IsUpgraded)
            await PowerCmd.Apply<AimPower>(choiceContext, Owner.Creature, DynamicVars[nameof(AimPower)].BaseValue,
                Owner.Creature, this);
    }
}