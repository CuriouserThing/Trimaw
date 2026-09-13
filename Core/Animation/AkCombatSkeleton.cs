using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Animation;

public class AkCombatSkeleton
{
    /// <summary>
    ///     Make sure this works! Just a dumb lil Reunion goon.
    /// </summary>
    private static readonly AkCombatSkeleton Fallback = Build("enemy_1002_nsabr",
        new ActionAnimation("Attack", 0.42, "Idle"));

    private readonly string _identifier;
    private readonly ActionAnimation _primaryAnimation;
    private readonly Dictionary<string, ActionAnimation> _secondaryAnimations = new();

    private AkCombatSkeleton(string identifier, ActionAnimation primaryAnimation)
    {
        _identifier = identifier;
        _primaryAnimation = primaryAnimation;
    }

    public string? StartId { get; private set; }
    public string IdleId { get; private set; } = "Idle";
    public string DieId { get; private set; } = "Die";


    public static AkCombatSkeleton Build(string identifier, ActionAnimation primaryAnimation)
    {
        return new AkCombatSkeleton(identifier, primaryAnimation);
    }

    public static AkCombatSkeleton BuildWithStart(string identifier, ActionAnimation primaryAnimation)
    {
        return new AkCombatSkeleton(identifier, primaryAnimation) { StartId = "Start" };
    }

    public AkCombatSkeleton WithStartId(string id)
    {
        StartId = id;
        return this;
    }

    public AkCombatSkeleton WithIdleId(string id)
    {
        IdleId = id;
        return this;
    }

    public AkCombatSkeleton WithDieId(string id)
    {
        DieId = id;
        return this;
    }

    public AkCombatSkeleton WithSecondaryAnimation(ActionAnimation animation)
    {
        return WithSecondaryAnimation(animation.ActionId, animation);
    }

    public AkCombatSkeleton WithSecondaryAnimation(string customId, ActionAnimation animation)
    {
        _secondaryAnimations.Add(customId, animation);
        return this;
    }

    public ActionAnimation GetAnimation(string? id)
    {
        if (id is null) return _primaryAnimation;
        if (_secondaryAnimations.TryGetValue(id, out var anim)) return anim;

        MainFile.Logger.Warn(
            $"Could not find secondary action animation {id} on skeleton {_identifier}. Using primary instead.");
        return _primaryAnimation;
    }

    public void LoadIntoSprite(MegaSprite sprite)
    {
        var skel = LoadResource();
        if (skel is null)
        {
            MainFile.Logger.Warn($"Could not load skeleton resource {_identifier}. Using fallback skel.");
            skel = Fallback.LoadResource();
            if (skel is null)
                throw new InvalidOperationException("Could not load skeleton fallback resource.");
        }
        else
        {
            MainFile.Logger.Info($"Loaded skeleton resource {_identifier}.");
        }

        sprite.SetSkeletonDataRes(skel);
    }

    private MegaSkeletonDataResource? LoadResource()
    {
        var path = Pathfinder.AkSkeletonDataRes(_identifier);
        var res = ResourceLoader.Load(path);
        return res is null ? null : new MegaSkeletonDataResource(res);
    }
}