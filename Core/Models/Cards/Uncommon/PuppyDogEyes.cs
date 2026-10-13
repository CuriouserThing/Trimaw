using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Enchantments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class PuppyDogEyes() : TrimawCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new[] { HoverTipFactory.FromPower<WeakPower>() }
        .Concat(HoverTipHelper.ForManifest<ReallyWet>());

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<WeakPower>(2),
        new(nameof(ReallyWet), 2)
    ];

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, DynamicVars[nameof(WeakPower)].BaseValue,
            Owner.Creature, this);

        var enchanter = new Enchanter<ReallyWet>(DynamicVars[nameof(ReallyWet)].IntValue);
        await EnchantCmd.Manifest(choiceContext, Owner, PileType.Hand, enchanter);
    }
}