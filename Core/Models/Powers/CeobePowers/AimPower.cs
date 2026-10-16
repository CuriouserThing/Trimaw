using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.CeobePowers;

public class AimPower : TrimawPower
{
    private const string AdditionalDamagePctKey = "AdditionalDamagePct";
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string Icon64Path => Pathfinder.NotoEmoji64("direct_hit");
    public override string Icon256Path => Pathfinder.NotoEmoji256("direct_hit");

    protected override IEnumerable<DynamicVar> CanonicalVars => [new(AdditionalDamagePctKey, 25)];

    private decimal AdditionalDamageMult => DynamicVars[AdditionalDamagePctKey].BaseValue / 100M;

    private int GetStacks()
    {
        return Math.Min(Amount, 1 + Owner.GetPowerAmount<BifocalPower>());
    }

    public override Task BeforeAttack(AttackCommand command)
    {
        if (command.Attacker != Owner ||
            command.CardPlay is not { } cardPlay ||
            cardPlay.Card.TargetType != TargetType.AnyEnemy) return Task.CompletedTask;

        Flash();
        var data = GetInternalData<Data>();
        data.CardPlayStacks[cardPlay] = GetStacks();
        return Task.CompletedTask;
    }

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props,
        Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (dealer != Owner ||
            cardSource is null ||
            cardSource.TargetType != TargetType.AnyEnemy) return 1;

        var data = GetInternalData<Data>();
        if (cardPlay is null || !data.CardPlayStacks.TryGetValue(cardPlay, out var stacks))
            // For card preview
            stacks = GetStacks();
        return 1 + stacks * AdditionalDamageMult;
    }

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (command.CardPlay is not { } cardPlay) return;
        var data = GetInternalData<Data>();
        if (!data.CardPlayStacks.Remove(cardPlay, out var stacks)) return;
        await PowerCmd.ModifyAmount(choiceContext, this, -stacks, Owner, null);
    }

    protected override object InitInternalData()
    {
        return new Data();
    }

    private class Data
    {
        public Dictionary<CardPlay, int> CardPlayStacks { get; } = [];
    }
}