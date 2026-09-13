using Godot;
using MegaCrit.Sts2.Core.Random;
using Trimaw.Core.SnackSystem;
using Trimaw.Core.Utils;
using Timer = Godot.Timer;

namespace Trimaw.Core.GodotControls;

public class MorselSlotWrapper : NodeWrapper<Node2D>
{
    private static readonly Dictionary<Morsel, string> MorselEmojiMap = new()
    {
        [SnackSystem.Morsel.Bread] = "baguette_bread",
        [SnackSystem.Morsel.Meat] = "meat_on_bone",
        [SnackSystem.Morsel.Shrooms] = "mushroom",
        [SnackSystem.Morsel.Berries] = "blueberries",
        [SnackSystem.Morsel.Pepper] = "hot_pepper",
        [SnackSystem.Morsel.Water] = "droplet"
    };

    private static readonly Dictionary<Morsel, MorselSlot> MorselSlots = new(SnackConstants.MorselKinds);

    private readonly Node2D _anchor;
    private readonly AnimationPlayer _animPlayer;
    private readonly Timer _animTimer;
    private readonly Rng _rng;
    private readonly Sprite2D _sprite;

    static MorselSlotWrapper()
    {
        for (var i = 0; i < SnackConstants.MorselKinds; i += 1)
        {
            var key = MorselEmojiMap[(Morsel)i];

            var scene = GD.Load<PackedScene>(Pathfinder.Scene("morsels", key));
            var morsel = scene.Instantiate<Node2D>();
            var anchor = morsel.GetNode<Node2D>("AnimationAnchor");
            var sprite = anchor.GetNode<Sprite2D>("MorselSprite");

            var animPlayer = morsel.GetNode<AnimationPlayer>("AnimationPlayer");
            var animLibrary = animPlayer.GetAnimationLibrary(string.Empty);

            var morselSlot = new MorselSlot(
                sprite.Texture,
                new Transform(anchor.Position, anchor.Rotation, anchor.Scale, anchor.Skew),
                new Transform(sprite.Position, sprite.Rotation, sprite.Scale, sprite.Skew),
                animLibrary);
            MorselSlots[(Morsel)i] = morselSlot;
        }
    }

    public MorselSlotWrapper(Node2D node) : base(node)
    {
        Transform.Clean.SetOnNode(node);

        _anchor = node.GetNode<Node2D>("AnimationAnchor");
        _sprite = _anchor.GetNode<Sprite2D>("MorselSprite");
        _sprite.Texture = null;

        _animPlayer = node.GetNode<AnimationPlayer>("AnimationPlayer");
        for (var i = 0; i < SnackConstants.MorselKinds; i += 1)
        {
            var m = (Morsel)i;
            _animPlayer.AddAnimationLibrary(MorselEmojiMap[m], MorselSlots[m].AnimLibrary);
        }

        _animTimer = node.GetNode<Timer>("AnimationTimer");
        _rng = Rng.Chaotic;
        Subscribe();
    }

    public Morsel? Morsel { get; private set; }

    public bool IsActiveWithinStock { get; set; }

    private void Subscribe()
    {
        _animTimer.Timeout += OnTimerTimeout;
        Node.TreeExiting += OnTreeExiting;
    }

    private void OnTreeExiting()
    {
        Node.TreeExiting -= OnTreeExiting;
        _animTimer.Timeout -= OnTimerTimeout;
    }

    private void OnTimerTimeout()
    {
        if (Morsel is not { } morsel || !IsActiveWithinStock) return;

        _animPlayer.Play($"{MorselEmojiMap[morsel]}/idle");
        _animTimer.Start(_animPlayer.CurrentAnimationLength + GetRandomDuration());
    }

    public void SetMorsel(Morsel morsel, Vector2 position, Vector2 scale)
    {
        Morsel = morsel;
        var morselSlot = MorselSlots[morsel];
        _sprite.Texture = morselSlot.SpriteTex;
        morselSlot.AnchorTransform.SetOnNode(_anchor);
        morselSlot.SpriteTransform.SetOnNode(_sprite);

        Node.Position = position;
        Node.Scale = 3 * scale;
        Node.Modulate = Colors.Transparent;

        const double entranceDuration = 0.5;

        var tween = Node.CreateTween();
        tween
            .TweenProperty(Node, "modulate", Colors.White, entranceDuration)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Cubic);
        tween.Parallel()
            .TweenProperty(Node, "scale", scale, entranceDuration)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Cubic);

        Node.Visible = true;
        _animTimer.Start(entranceDuration + GetRandomDuration());
    }

    // Wait (in seconds) between end of one idle animation and start of the next
    private double GetRandomDuration()
    {
        var r = _rng.NextGaussianInt(200, 50, 50, 400);
        return r / 100.0;
    }

    public class Transform(Vector2 position, float rotation, Vector2 scale, float skew)
    {
        public Vector2 Position { get; } = position;
        public float Rotation { get; } = rotation;
        public Vector2 Scale { get; } = scale;
        public float Skew { get; } = skew;

        public static Transform Clean { get; } = new(Vector2.Zero, 0f, Vector2.One, 0f);

        public void SetOnNode(Node2D node)
        {
            node.Position = Position;
            node.Rotation = Rotation;
            node.Scale = Scale;
            node.Skew = Skew;
        }
    }

    private class MorselSlot(
        Texture2D spriteTex,
        Transform anchorTransform,
        Transform spriteTransform,
        AnimationLibrary animLibrary)
    {
        public Texture2D SpriteTex { get; } = spriteTex;
        public Transform AnchorTransform { get; } = anchorTransform;
        public Transform SpriteTransform { get; } = spriteTransform;
        public AnimationLibrary AnimLibrary { get; } = animLibrary;
    }
}