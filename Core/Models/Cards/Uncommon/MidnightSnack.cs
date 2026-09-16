using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.CombatHistory;
using Trimaw.Core.Commands;
using Trimaw.Core.Hooks;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core.Models.Cards.Uncommon;

public class MidnightSnack() : TrimawCard(12, CardType.Skill, CardRarity.Uncommon, TargetType.Self), IPrepListener
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Prep)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new(nameof(Morsel), 4)];

    public Task AfterMorselPrepped(PlayerChoiceContext choiceContext, Player player, Morsel morsel,
        SnackCard? createdSnack)
    {
        if (player != Owner) return Task.CompletedTask;

        EnergyCost.AddThisCombat(-1);
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || IsClone) return Task.CompletedTask;

        var morselsPrepped = MainFile.CombatManagerFactory.GetOrCreate(Owner).AllHistoryEntries
            .OfType<MorselPreppedEntry>()
            .Count();
        EnergyCost.AddThisCombat(-morselsPrepped);
        return Task.CompletedTask;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var morsels = DynamicVars[nameof(Morsel)].IntValue;
        for (var i = 0; i < morsels; i += 1) await SnackCmd.PrepRandom(choiceContext, CombatState, Owner);
    }
}