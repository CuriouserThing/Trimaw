using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Powers.CeobePowers;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class FlushOut() : TrimawCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<VigorPower>(),
        HoverTipFactory.FromPower<AimPower>(),
    ];

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var aim = Owner.Creature.GetPowerAmount<AimPower>();
        if (aim > 0)
            await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, aim, Owner.Creature, this);
    }
}