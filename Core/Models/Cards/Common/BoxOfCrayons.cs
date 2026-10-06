using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Enchantments;

namespace Trimaw.Core.Models.Cards.Common;

public class BoxOfCrayons() : TrimawCard(1, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Doodled>();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(1, ValueProp.Move),
        new RepeatVar(2),
    ];

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1);
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

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

        var doodled = ModelDb.Enchantment<Doodled>();
        var card = CardFactory.GetDistinctForCombat(
            Owner,
            Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
                .Where(doodled.CanEnchant),
            1,
            Owner.RunState.Rng.CombatCardGeneration).First();
        CardCmd.Enchant<Doodled>(card, 1);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }
}