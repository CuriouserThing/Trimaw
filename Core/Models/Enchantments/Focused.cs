using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Models.Powers.CeobePowers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class Focused : TrimawEnchantment
{
    private int _trackedAim;
    private AttackCommand? _trackedAttack;
    private int _trackedVigor;

    public override bool HasExtraCardText => true;
    public override string Icon64Path => Pathfinder.GameIconsDotnet64("eye_target");

    public override bool CanEnchantCardType(CardType cardType)
    {
        return cardType == CardType.Attack;
    }

    public override bool CanEnchant(CardModel card)
    {
        // Only enchant targeted Attacks, since untargeted Attacks can never consume Aim regardless.
        return base.CanEnchant(card) && card.TargetType == TargetType.AnyEnemy;
    }

    public override Task BeforeAttack(AttackCommand command)
    {
        if (IsMutable && command.ModelSource == Card)
        {
            _trackedAttack = command;
            _trackedAim = command.Attacker?.GetPowerAmount<AimPower>() ?? 0;
            _trackedVigor = command.Attacker?.GetPowerAmount<VigorPower>() ?? 0;
        }

        return Task.CompletedTask;
    }

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (command == _trackedAttack && command.Attacker is { } attacker)
        {
            await ApplyPower<AimPower>(choiceContext, attacker, _trackedAim);
            await ApplyPower<VigorPower>(choiceContext, attacker, _trackedVigor);
        }

        _trackedAttack = null;
        _trackedAim = 0;
        _trackedVigor = 0;
    }

    private async Task ApplyPower<T>(PlayerChoiceContext choiceContext, Creature creature, int trackedPower)
        where T : PowerModel
    {
        var current = creature.GetPowerAmount<T>();
        if (trackedPower < current) return;
        var delta = trackedPower - current;
        await PowerCmd.Apply<T>(choiceContext, creature, delta, creature, Card);
    }
}