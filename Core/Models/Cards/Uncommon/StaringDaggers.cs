using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Cards.Tokens;
using Trimaw.Core.Models.Powers.CeobePowers;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class StaringDaggers() : TrimawCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<Dart>(),
        HoverTipFactory.FromPower<AimPower>()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<VulnerablePower>(2),
        new(nameof(Dart), 2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target,
            DynamicVars[nameof(VulnerablePower)].BaseValue, Owner.Creature, this);

        ArgumentNullException.ThrowIfNull(CombatState);
        var dartCount = DynamicVars[nameof(Dart)].IntValue;
        var darts = new CardModel[dartCount];
        for (var i = 0; i < dartCount; i += 1)
        {
            var dart = CombatState.CreateCard<Dart>(Owner);
            if (IsUpgraded) CardCmd.Upgrade(dart);
            darts[i] = dart;
        }

        await CardPileCmd.AddGeneratedCardsToCombat(darts, PileType.Hand, Owner);
    }
}