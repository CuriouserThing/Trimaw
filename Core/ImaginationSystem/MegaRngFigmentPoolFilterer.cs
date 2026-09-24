using System.Drawing;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Random;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Models.Powers;

namespace Trimaw.Core.ImaginationSystem;

public class MegaRngFigmentPoolFilterer(IReadOnlyList<Figment> figmentPool, Rng rng) : IFigmentFilterer
{
    // +1 spotlight Y to Y-sort over player
    // Deathbeds 100% higher than all idle Y levels
    private static readonly FigmentSlotMap DefaultFigmentMap = new(
        new FigmentSlot(
            new Vector2(-210, -55),
            new Vector2(-200, +1),
            new RectangleF(-200, -81, 100, 10)),
        new FigmentSlot(
            new Vector2(+160, -70),
            new Vector2(+230, +1),
            new RectangleF(+180, -81, 100, 10)),
        new FigmentSlot(
            new Vector2(+350, -50),
            new Vector2(+240, +1),
            new RectangleF(+220, -81, 100, 10)));

    private readonly Queue<Figment> _queue = new();
    private bool _activeImagination;

    public Figment? FilterFigment(Player owner, FigmentPower power)
    {
        var candidates = figmentPool.Where(f => f.StartsWithPower(power));
        return rng.NextItem(candidates);
    }

    public async Task ImagineFigment(PlayerChoiceContext choiceContext, Player owner, Figment figment)
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
            await ImaginationCmd.Imagine(choiceContext, owner, nextFigment, DefaultFigmentMap);
        _activeImagination = false;
    }
}