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

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target,
        DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        var damage = result.TotalDamage + result.OverkillDamage;
        if (dealer is null || dealer != Owner.PetOwner?.Creature ||
            damage < DynamicVars[nameof(DamageThreshold)].BaseValue) return;

        await TriggerMove(choiceContext, new MoveParams { Addend = damage, Target = target });
    }
}