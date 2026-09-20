using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Trimaw.Core.Models.Powers.CeobePowers;

namespace Trimaw.Core.Models.Cards.Rare;

public class CerberusForm() : TrimawCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    private const string MorselsDevouredKey = "MorselsDevoured";

    protected override IEnumerable<DynamicVar> CanonicalVars => [new(MorselsDevouredKey, 3)];

    protected override void OnUpgrade()
    {
        DynamicVars[MorselsDevouredKey].UpgradeValueBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<CerberusFormPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}