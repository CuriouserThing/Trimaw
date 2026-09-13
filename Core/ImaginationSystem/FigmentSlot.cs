using System.Drawing;
using Godot;
using MegaCrit.Sts2.Core.Random;

namespace Trimaw.Core.ImaginationSystem;

public class FigmentSlot(Vector2 idlePosition, Vector2 spotlightPosition, RectangleF deathbedBounds)
{
    public Vector2 IdlePosition => idlePosition;

    public Vector2 SpotlightPosition => spotlightPosition;

    private RectangleF DeathbedBounds => deathbedBounds;

    public Vector2 GetRandomDeathbedPosition()
    {
        var bounds = DeathbedBounds;
        var x = bounds.X + bounds.Width * Rng.Chaotic.NextGaussianDouble(0.5, 1.0 / 6.0);
        var y = bounds.Y + bounds.Height * Rng.Chaotic.NextGaussianDouble(0.5, 1.0 / 6.0);
        return new Vector2((float)x, (float)y);
    }
}