using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Commands;
using Trimaw.Core.ImaginationSystem;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class FogMog() : TrimawCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Pop)];

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var figments = Owner.GetFigments().ToArray();
        var hpLoss = 0;
        foreach (var figment in figments)
        {
            figment.MarkForPopping();
            hpLoss += figment.Creature.CurrentHp;
        }

        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        var damage = new DamageVar(hpLoss, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move);
        await CreatureCmd.Damage(choiceContext, cardPlay.Target, damage, this, cardPlay);

        foreach (var figment in figments)
        {
            await figment.UseMove(choiceContext, MoveParams.None, TriggerKind.Pop);
            await figment.Pop();
        }
    }
}