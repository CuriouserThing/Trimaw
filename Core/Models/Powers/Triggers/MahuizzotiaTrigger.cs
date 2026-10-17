using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.Triggers;

public class MahuizzotiaTrigger : FigmentMonoTrigger
{
    public const decimal DamageThreshold = 10;

    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("rally_the_troops");
    public override string Icon256Path => Pathfinder.GameIconsDotnet256("rally_the_troops");

    protected override IEnumerable<DynamicVar> CanonicalVars => [new(nameof(DamageThreshold), DamageThreshold)];

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target,
        DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        var damage = result.TotalDamage + result.OverkillDamage;
        if (dealer is null || dealer != Owner.PetOwner?.Creature ||
            cardSource is null ||
            damage < DynamicVars[nameof(DamageThreshold)].BaseValue) return Task.CompletedTask;

        var data = GetInternalData<Data>();
        if (damage < data.MaxDamageSeen) return Task.CompletedTask;

        data.CardSource = cardSource;
        data.MaxDamageSeen = damage;
        data.Target = target;
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Player != Owner.PetOwner) return;

        var data = GetInternalData<Data>();
        if (cardPlay.Card != data.CardSource) return; // is there a better/safer way to track card-play damage...?

        await TriggerMove(choiceContext, new MoveParams { BaseAmount = data.MaxDamageSeen, EnemyTarget = data.Target });
    }

    private class Data
    {
        public CardModel? CardSource { get; set; }

        public int MaxDamageSeen { get; set; }

        public Creature? Target { get; set; }
    }
}