using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Enchantments;

public class Doodled : TrimawEnchantment
{
    private const string DamageReductionPctKey = "DamageReductionPct";
    public override string Icon64Path => Pathfinder.NotoEmoji64("lower_left_crayon");

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1),
        new(DamageReductionPctKey, 50)
    ];

    public override bool CanEnchant(CardModel card)
    {
        return base.CanEnchant(card) &&
               card.EnergyCost.Canonical > 1 &&
               (card.Type == CardType.Attack || card.GainsBlock);
    }

    protected override void OnEnchant()
    {
        Card.EnergyCost.SetThisCombat(1);
    }

    public override decimal EnchantDamageMultiplicative(decimal originalDamage, ValueProp props)
    {
        return DynamicVars[DamageReductionPctKey].BaseValue / 100M;
    }

    public override decimal EnchantBlockMultiplicative(decimal originalBlock)
    {
        return DynamicVars[DamageReductionPctKey].BaseValue / 100M;
    }
}