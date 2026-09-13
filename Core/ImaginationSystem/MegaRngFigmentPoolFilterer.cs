using System.Drawing;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Random;
using Trimaw.Core.Commands;

namespace Trimaw.Core.ImaginationSystem;

public class MegaRngFigmentPoolFilterer(
    IReadOnlyList<TaggedFigment> figmentPool,
    FigmentFilter standardPrepass,
    FigmentFilter gachaPrepass,
    Rng rng) : IFigmentFilterer
{
    // +1 spotlight Y to Y-sort over player
    // Deathbeds 100% higher than all idle Y levels
    private static readonly FigmentSlotMap DefaultFigmentMap = new(
        new FigmentSlot(
            new Vector2(-210, -55),
            new Vector2(-200, +1),
            new RectangleF(-200, -71, 100, 10)),
        new FigmentSlot(
            new Vector2(+160, -70),
            new Vector2(+230, +1),
            new RectangleF(+180, -80, 100, 10)),
        new FigmentSlot(
            new Vector2(+350, -50),
            new Vector2(+240, +1),
            new RectangleF(+220, -71, 100, 10)));

    private readonly Queue<TaggedFigment> _queue = new();
    private bool _activeImagination;

    public TaggedFigment GachaPullFigment(Player owner)
    {
        return FilterFigment(owner, FigmentFilter.All.Applying(gachaPrepass));
    }

    public TaggedFigment FilterFigment(Player owner, FigmentFilter filter)
    {
        var existingFigmentTypes = owner.GetFigments()
            .Select(f => f.GetType())
            .Distinct()
            .ToHashSet();
        filter = FigmentFilter.All
            .Applying(standardPrepass)
            .Applying(filter);

        var count = figmentPool.Count;
        var weights = new decimal[count];
        var totalWeight = 0.0M;
        for (var i = 0; i < count; i += 1)
        {
            var figment = figmentPool[i];
            var weight = existingFigmentTypes.Contains(figment.FigmentType) ? 0 : filter.GetMult(figment);
            weights[i] = weight;
            totalWeight += weight;
        }

        var n = (decimal)rng.NextDouble(0.0, (double)totalWeight);
        n = Math.Clamp(n, 0.0M, totalWeight);

        var w = 0.0M;
        for (var i = 0; i < count; i += 1)
        {
            w += weights[i];
            if (w > n) return figmentPool[i];
        }

        // Astronomical chance (unless bug); just return last if here
        return figmentPool[^1];
    }

    public async Task ImagineFigment(PlayerChoiceContext choiceContext, Player owner, TaggedFigment figment)
    {
        _queue.Enqueue(figment);

        // Obv not thread-safe but obv we don't care
        if (_activeImagination)
        {
            MainFile.Logger.Info($"Already in the process of imagining a figment. Enqueueing {figment} for afterward.");
            return;
        }

        _activeImagination = true;
        while (_queue.TryDequeue(out var nextFigment))
            await nextFigment.Imagine(choiceContext, owner, DefaultFigmentMap);
        _activeImagination = false;
    }
}