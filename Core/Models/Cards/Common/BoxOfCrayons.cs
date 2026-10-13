using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Enchantments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Common;

public class BoxOfCrayons() : TrimawCard(1, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Doodled>();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(1, ValueProp.Move),
        new RepeatVar(2)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);
        await TrimawCmd
            .Attack(this, cardPlay, DynamicVars.Damage.BaseValue)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .TargetingRandomOpponents(CombatState)
            .WithKnifeAnimation()
            .WithTimescale(1.25f)
            .Execute(choiceContext);

        var card = await EnchantCmd.GenerateCard(choiceContext, Owner, new Enchanter<Doodled>());
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }
}