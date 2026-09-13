using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class ReallyHeavy : TrimawEnchantment
{
    private const string DamageDivisorKey = "DamageDivisor";
    public override string Icon64Path => Pathfinder.GameIconsDotnet64("weight");

    protected override IEnumerable<DynamicVar> CanonicalVars => [new(DamageDivisorKey, 7)];

    public override bool CanEnchantCardType(CardType cardType)
    {
        return cardType == CardType.Attack;
    }

    public override bool CanEnchant(CardModel card)
    {
        return base.CanEnchant(card) &&
               card.DynamicVars.Damage.BaseValue >= DynamicVars[DamageDivisorKey].BaseValue;
    }

    public override decimal EnchantDamageAdditive(decimal originalDamage, ValueProp props)
    {
        var strength = Card.Owner.Creature.GetPowerAmount<StrengthPower>();
        var vigor = Card.Owner.Creature.GetPowerAmount<VigorPower>();
        var mult = originalDamage / DynamicVars[DamageDivisorKey].BaseValue;
        return (strength + vigor) * mult;
    }
}