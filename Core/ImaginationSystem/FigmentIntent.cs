using Godot;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Intents;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Nodes.Combat;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem;

/// <summary>
///     Base class for the intents of all figment moves. Currently, the vanilla STS2 intent code has two problems for us:
///     1) it's oriented toward enemy monster intents, and
///     2) it has a smattering of hardcoded behavior incompatible with our needs.
///     This class re-bases <see cref="AbstractIntent" /> into something we can use, within the limitations of the intent
///     system.
/// </summary>
/// <remarks>We use <see cref="StatusIntent" /> due to <see cref="NIntent" /> whitelisting it for intent label display.</remarks>
public abstract class FigmentIntent() : StatusIntent(0)
{
    protected sealed override string IntentPrefix
    {
        get
        {
            var modId = StringHelper.Slugify(MainFile.ModId);
            var name = StringHelper.Slugify(GetType().Name);
            var slug = $"{modId}-{name}";

            const string suffix = "_INTENT";
            if (slug.EndsWith(suffix)) slug = slug.Remove(slug.Length - suffix.Length);

            return slug;
        }
    }

    // STS2 appends this to a hardcoded location, so it's unhelpful to us
#pragma warning disable CS8764
    protected sealed override string? SpritePath => null;
#pragma warning restore CS8764

    public sealed override IntentType IntentType => DefaultVanillaIntent.Type;

#pragma warning disable CS8764
    protected sealed override LocString? IntentLabelFormat => null;
#pragma warning restore CS8764

    // Necessary for figments
    public sealed override bool HasIntentTip => true;

    public override IEnumerable<string> AssetPaths => [DefaultTipIconPath];

    private protected abstract VanillaIntentWrapper DefaultVanillaIntent { get; }

    private protected abstract bool HasIntentLabel { get; }

    protected virtual string DefaultTipIconPath => Pathfinder.NotoEmoji64("construction");

    internal abstract string? GetAnimationId(
        Figment moveUser,
        Player owner,
        MoveParams moveParams);

    internal abstract bool CanPerform(
        Figment moveUser,
        Player owner,
        MoveParams moveParams,
        out Creature? visualTarget);

    internal abstract Task BeforePerform(
        Figment moveUser,
        Player owner,
        MoveParams moveParams,
        PlayerChoiceContext choiceCtx);

    internal abstract Task<FigmentMoveResult> PerformMove(
        Figment moveUser,
        Player owner,
        MoveParams moveParams,
        PlayerChoiceContext choiceCtx);

    internal abstract Task AfterPerform(
        Figment moveUser,
        Player owner,
        MoveParams moveParams,
        PlayerChoiceContext choiceCtx);

    /// <summary>
    ///     Bodged wrapper joining <see cref="IntentType" /> and the hardcoded animation string ID, which are almost but not
    ///     quite 1:1.
    /// </summary>
    private protected class VanillaIntentWrapper
    {
        public static readonly VanillaIntentWrapper Debuff = new(IntentAnimData.debuff, IntentType.Debuff);
        public static readonly VanillaIntentWrapper DebuffStrong = new(IntentAnimData.debuff, IntentType.DebuffStrong);

        /// <summary>
        ///     Knife
        /// </summary>
        public static readonly VanillaIntentWrapper Attack1 = new(IntentAnimData.attack1, IntentType.Attack);

        /// <summary>
        ///     Kris
        /// </summary>
        public static readonly VanillaIntentWrapper Attack2 = new(IntentAnimData.attack2, IntentType.Attack);

        /// <summary>
        ///     Sword
        /// </summary>
        public static readonly VanillaIntentWrapper Attack3 = new(IntentAnimData.attack3, IntentType.Attack);

        /// <summary>
        ///     Saber
        /// </summary>
        public static readonly VanillaIntentWrapper Attack4 = new(IntentAnimData.attack4, IntentType.Attack);

        /// <summary>
        ///     Scythe
        /// </summary>
        public static readonly VanillaIntentWrapper Attack5 = new(IntentAnimData.attack5, IntentType.Attack);

        // Everything else is 1:1
        public static readonly VanillaIntentWrapper Buff = new(IntentAnimData.buff, IntentType.Buff);
        public static readonly VanillaIntentWrapper CardDebuff = new(IntentAnimData.cardDebuff, IntentType.CardDebuff);
        public static readonly VanillaIntentWrapper DeathBlow = new(IntentAnimData.deathBlow, IntentType.DeathBlow);
        public static readonly VanillaIntentWrapper Defend = new(IntentAnimData.defend, IntentType.Defend);
        public static readonly VanillaIntentWrapper Escape = new(IntentAnimData.escape, IntentType.Escape);
        public static readonly VanillaIntentWrapper Heal = new(IntentAnimData.heal, IntentType.Heal);
        public static readonly VanillaIntentWrapper Hidden = new(IntentAnimData.hidden, IntentType.Hidden);
        public static readonly VanillaIntentWrapper Sleep = new(IntentAnimData.sleep, IntentType.Sleep);
        public static readonly VanillaIntentWrapper Status = new(IntentAnimData.status, IntentType.StatusCard);
        public static readonly VanillaIntentWrapper Stun = new(IntentAnimData.stun, IntentType.Stun);
        public static readonly VanillaIntentWrapper Summon = new(IntentAnimData.summon, IntentType.Summon);
        public static readonly VanillaIntentWrapper Unknown = new(IntentAnimData.unknown, IntentType.Unknown);

        private VanillaIntentWrapper(string anim, IntentType type)
        {
            Anim = anim;
            Type = type;
        }

        public string Anim { get; }
        public IntentType Type { get; }
    }

    #region Formatting

    protected void FormatWithBlock(LocString str, MoveContext ctx, Creature target, BlockVar block)
    {
        var value = Hook.ModifyBlock(
            ctx.CombatState,
            target,
            block.BaseValue,
            block.Props,
            null,
            null,
            out var modifiers);
        str.Add(new BlockVar(block.Name, value, block.Props));
    }

    protected void FormatWithTargetedDamage(LocString str, MoveContext ctx, Creature? target, DamageVar damage)
    {
        str.Add(new DamageVar(
            damage.Name,
            ModifyDamage(ctx, target, damage),
            damage.Props));
    }

    protected void FormatWithAnyCreatureDamage(LocString str, MoveContext ctx, DamageVar damage)
    {
        str.Add(new DamageVar(
            damage.Name,
            ModifyMultiCreatureDamage(ctx, damage),
            damage.Props));
    }

    protected void FormatWithAttackHitCount(LocString str, MoveContext ctx, AttackCommand cmd, RepeatVar hitCount)
    {
        str.Add(new RepeatVar(
            hitCount.Name,
            (int)Hook.ModifyAttackHitCount(ctx.CombatState, cmd, hitCount.IntValue)));
    }

    private decimal ModifyDamage(MoveContext ctx, Creature? target, DamageVar damage)
    {
        return Hook.ModifyDamage(
            ctx.PetOwner.RunState,
            ctx.CombatState,
            target,
            ctx.MoveUser.Creature,
            damage.BaseValue,
            damage.Props,
            null,
            null,
            ModifyDamageHookType.All,
            CardPreviewMode.Normal,
            out var modifiers);
    }

    private decimal ModifyMultiCreatureDamage(MoveContext ctx, DamageVar damage)
    {
        // This is more or less vanilla logic: if all hittable targets would receive the exact same damage,
        // return that value; otherwise, return the value calculated with a null target.
        // If Hook.ModifyDamage ever allows us to use it for pet multi-creature attacks, scrap this method.

        decimal? commonAmount = null;
        foreach (var target in ctx.CombatState.HittableEnemies)
        {
            var currentAmount = ModifyDamage(ctx, target, damage);
            if (commonAmount is { } prevAmount && currentAmount != prevAmount)
            {
                commonAmount = null;
                break;
            }

            commonAmount = currentAmount;
        }

        return commonAmount ?? ModifyDamage(ctx, null, damage);
    }

    #endregion
}

public abstract class FigmentIntent<T> : FigmentIntent where T : Figment
{
    private static MoveContext<T> CreateContext(T moveUser, Player petOwner, MoveParams? moveParams = null)
    {
        moveParams = moveUser.Creature.Powers.OfType<FigmentTalent>()
            .Aggregate(moveParams ?? MoveParams.None, (current, talent) => talent.ModifyMoveParams(current, true));
        return new MoveContext<T>(moveUser, petOwner, moveParams);
    }

    private static async Task<MoveContext<T>> CreateHardContext(T moveUser, Player petOwner, MoveParams moveParams)
    {
        // Can't enumerate normally because talents may remove themselves
        var talents = moveUser.Creature.Powers.OfType<FigmentTalent>().ToArray();
        foreach (var talent in talents)
        {
            var newParams = talent.ModifyMoveParams(moveParams, false);
            await talent.AfterModifyingMoveParams(moveParams, newParams);
            moveParams = newParams;
        }

        return new MoveContext<T>(moveUser, petOwner, moveParams);
    }

    // We do this in place of SpritePath
    public sealed override Texture2D? GetTexture(IEnumerable<Creature> targets, Creature owner)
    {
        var path = owner is { Monster: T moveUser, PetOwner: { } petOwner }
            ? GetCurrentTipIconPath(CreateContext(moveUser, petOwner))
            : DefaultTipIconPath;
        return ResourceLoader.Load<Texture2D>(path);
    }

    // Don't mess with this unless STS2 refactors anim hardcoding or redesigns intent anim in general
    public sealed override string GetAnimation(IEnumerable<Creature> targets, Creature owner)
    {
        if (owner is { Monster: T moveUser, PetOwner: { } petOwner })
            return GetCurrentVanillaIntent(CreateContext(moveUser, petOwner)).Anim;

        return DefaultVanillaIntent.Anim;
    }

    // STS2 hardcodes the title LocString location already, so just use the default description LocString location
    protected sealed override LocString GetIntentDescription(IEnumerable<Creature> targets, Creature owner)
    {
        var desc = base.GetIntentDescription(targets, owner);

        if (owner is { Monster: T moveUser, PetOwner: { } petOwner })
            FormatTipDescription(desc, CreateContext(moveUser, petOwner));

        return desc;
    }

    public sealed override LocString GetIntentLabel(IEnumerable<Creature> targets, Creature owner)
    {
        if (HasIntentLabel && owner is { Monster: T moveUser, PetOwner: { } petOwner })
        {
            var label = new LocString(_locTable, $"{IntentPrefix}.label"); // custom label field
            FormatIntentLabel(label, CreateContext(moveUser, petOwner));
            return label;
        }

        return new LocString(_locTable, "FORMAT_EMPTY"); // should get empty label
    }

    private protected virtual VanillaIntentWrapper GetCurrentVanillaIntent(MoveContext<T> ctx)
    {
        return DefaultVanillaIntent;
    }

    protected virtual void FormatIntentLabel(LocString label, MoveContext<T> ctx)
    {
    }

    protected virtual string GetCurrentTipIconPath(MoveContext<T> ctx)
    {
        return DefaultTipIconPath;
    }

    protected virtual void FormatTipDescription(LocString desc, MoveContext<T> ctx)
    {
    }

    internal sealed override string? GetAnimationId(
        Figment moveUser,
        Player owner,
        MoveParams moveParams)
    {
        if (moveUser is not T moveUserSpecial)
        {
            MainFile.Logger.Warn(
                $"Figment of type {moveUser.GetType()} has an intent with mismatched figment type {typeof(T)}.");
            return null;
        }

        var ctx = CreateContext(moveUserSpecial, owner, moveParams);
        return GetAnimationId(ctx);
    }

    internal sealed override bool CanPerform(
        Figment moveUser,
        Player owner,
        MoveParams moveParams,
        out Creature? visualTarget)
    {
        visualTarget = null;
        if (moveUser is not T moveUserSpecial)
        {
            MainFile.Logger.Warn(
                $"Figment of type {moveUser.GetType()} has an intent with mismatched figment type {typeof(T)}.");
            return false;
        }

        var ctx = CreateContext(moveUserSpecial, owner, moveParams);
        return CanPerform(ctx, out visualTarget);
    }

    internal sealed override async Task BeforePerform(Figment moveUser, Player owner, MoveParams moveParams,
        PlayerChoiceContext choiceCtx)
    {
        if (moveUser is not T moveUserSpecial) return;
        var ctx = CreateContext(moveUserSpecial, owner, moveParams);
        await BeforePerform(ctx, choiceCtx);
    }

    internal sealed override async Task<FigmentMoveResult> PerformMove(
        Figment moveUser,
        Player owner,
        MoveParams moveParams,
        PlayerChoiceContext choiceCtx)
    {
        if (moveUser is not T moveUserSpecial) return FigmentMoveResult.MismatchedFigmentType;
        var ctx = await CreateHardContext(moveUserSpecial, owner, moveParams);
        return await OnPerform(ctx, choiceCtx);
    }

    internal sealed override async Task AfterPerform(Figment moveUser, Player owner, MoveParams moveParams,
        PlayerChoiceContext choiceCtx)
    {
        if (moveUser is not T moveUserSpecial) return;
        var ctx = CreateContext(moveUserSpecial, owner, moveParams);
        await AfterPerform(ctx, choiceCtx);
    }

    /// <summary>
    ///     Gets the action animation ID for this intent's move. Null by default, representing the primary action animation.
    /// </summary>
    protected virtual string? GetAnimationId(MoveContext<T> ctx)
    {
        return null;
    }

    /// <summary>
    ///     Override if the intent may need to return false to report the move is unperformable, or to specify a visual target
    ///     ahead of time (to help the animator etc.)
    /// </summary>
    protected virtual bool CanPerform(MoveContext<T> ctx, out Creature? visualTarget)
    {
        visualTarget = null;
        return true;
    }

    protected virtual Task BeforePerform(MoveContext<T> ctx, PlayerChoiceContext choiceCtx)
    {
        return Task.CompletedTask;
    }

    protected abstract Task<FigmentMoveResult> OnPerform(MoveContext<T> ctx, PlayerChoiceContext choiceCtx);

    protected virtual Task AfterPerform(MoveContext<T> ctx, PlayerChoiceContext choiceCtx)
    {
        return Task.CompletedTask;
    }
}