using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Powers.CeobePowers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Powers.SharedTalents;

public class AimShareTalent : FigmentTalentPower
{
    public override PowerStackType StackType => PowerStackType.Single;

    public override string Icon64Path => Pathfinder.GameIconsDotnet64("target_dummy");
    public override string Icon256Path => Pathfinder.GameIconsDotnet64("target_dummy");

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<AimPower>()];

    // General note: unlike with Aim itself, we have to assume all damage from this figment should use & consume Aim
    // (Which it logically should, given we have control over which figments get this talent)
    private AimPower? Aim => Figment.PetOwner.Creature.GetPower<AimPower>();

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props,
        Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (dealer != Owner) return 1;

        return 1 + (Aim is { } aim ? aim.AdditionalDamageMult : 0);
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer,
        DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer != Owner) return;

        if (Aim is { } aim) await PowerCmd.Decrement(aim);
    }
}