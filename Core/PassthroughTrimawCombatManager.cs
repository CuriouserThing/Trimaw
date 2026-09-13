using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Animation;
using Trimaw.Core.CombatHistory;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core;

/// <summary>
///     No-fuss combat manager that only delegates to its constituents.
/// </summary>
public class PassthroughTrimawCombatManager(
    IPrepManager prepManager,
    IFigmentFilterer figmentFilterer,
    ISuperSpecializer superSpecializer,
    ICeobeAnimator ceobeAnimator) : ITrimawCombatManager
{
    private readonly List<TrimawCombatHistoryEntry> _entries = [];

    public IReadOnlyList<TrimawCombatHistoryEntry> AllHistoryEntries => _entries;

    public void AddHistoryEntry(TrimawCombatHistoryEntry entry)
    {
        _entries.Add(entry);
    }

    public void ResetPrepState()
    {
        prepManager.ResetPrepState();
    }

    public IReadOnlyList<Morsel> CurrentPrep => prepManager.CurrentPrep;

    public Morsel? PreviousMorsel => prepManager.PreviousMorsel;

    public SnackResult? AddMorsel(Morsel morsel)
    {
        return prepManager.AddMorsel(morsel);
    }

    public Morsel GenerateRandomMorsel()
    {
        return prepManager.GenerateRandomMorsel();
    }

    public TaggedFigment FilterFigment(Player owner, FigmentFilter filter)
    {
        return figmentFilterer.FilterFigment(owner, filter);
    }

    public TaggedFigment GachaPullFigment(Player owner)
    {
        return figmentFilterer.GachaPullFigment(owner);
    }

    public async Task ImagineFigment(PlayerChoiceContext choiceContext, Player owner, TaggedFigment figment)
    {
        await figmentFilterer.ImagineFigment(choiceContext, owner, figment);
    }

    public void MakeSuperSpecial(CardModel card)
    {
        superSpecializer.MakeSuperSpecial(card);
    }

    public Task<CeobeAnimationResult> AnimateAttack(
        MegaAnimationState state,
        CeobeAttack attack,
        int hitCount,
        bool spaceHitsEvenly,
        float timescale)
    {
        return ceobeAnimator.AnimateAttack(state, attack, hitCount, spaceHitsEvenly, timescale);
    }
}