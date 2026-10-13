using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using Trimaw.Core.Models.Powers.CeobePowers;

namespace Trimaw.Core.Models.Cards.Rare;

public class StoneDisease() : TrimawCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Instinct>();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(1)
    ];

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<StoneDiseasePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);

        await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue, true);
    }
}