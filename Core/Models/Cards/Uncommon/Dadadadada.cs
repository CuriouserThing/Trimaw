using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Models.Cards.Tokens;
using Trimaw.Core.Models.Powers.CeobePowers;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class Dadadadada() : TrimawCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Dart>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new(nameof(Dart), 2)];

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(Dart)].UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);

        await PowerCmd.Apply<DadadadadaPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);

        var dartCount = DynamicVars[nameof(Dart)].IntValue;
        var darts = new CardModel[dartCount];
        for (var i = 0; i < dartCount; i += 1)
            darts[i] = CombatState.CreateCard<Dart>(Owner);
        await CardPileCmd.AddGeneratedCardsToCombat(darts, PileType.Hand, Owner);
    }
}