using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Powers.CeobePowers;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class ThickFat() : TrimawCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5, ValueProp.Unpowered)];

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<ThickFatPower>(choiceContext, Owner.Creature, DynamicVars.Block.BaseValue, Owner.Creature,
            cardPlay.Card);
    }
}