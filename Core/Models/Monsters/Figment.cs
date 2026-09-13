using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Utils;
using Timer = Trimaw.Core.Animation.Timer;

namespace Trimaw.Core.Models.Monsters;

public abstract class Figment : CustomMonsterModel
{
    private IActionAnimator _actionAnimator = EmptyActionAnimator.Instance;
    private bool _hasPopped;
    private bool _markedForPopping;
    private Player? _petOwner;
    private Timer? _tweenTimer;

    protected abstract AkCombatSkeleton Skeleton { get; }

    public abstract IReadOnlySet<HardTag> HardTags { get; }

    protected abstract int InitialHp { get; }
    protected virtual int MaxHp => 4 * InitialHp;
    public sealed override int MinInitialHp => InitialHp;
    public sealed override int MaxInitialHp => InitialHp;

    public Player PetOwner => _petOwner ?? throw new InvalidOperationException(
        $"Can't access {nameof(PetOwner)} before calling {nameof(Initialize)}.");

    /// <summary>
    ///     The highest HP that this figment has seen its owner possess. For healing purposes.
    /// </summary>
    public int OwnerHpThreshold { get; private set; }

    protected virtual int InitialLifecyclePowerAmount => 1;
    protected virtual int InitialTalentPowerAmount => 1;

    public int MovesUsed { get; private set; }

    public uint MinRemainingMoves { get; private set; }
    public uint? MaxRemainingMoves { get; private set; }

    public FigmentIntent? CurrentIntent { get; private set; }

    public bool NoAnimationPending => _actionAnimator.AllAnimationsHaveFinished &&
                                      (_tweenTimer is null || !_tweenTimer.TimerRunning);

    public bool IsAvailable => !Creature.IsDead && !_markedForPopping;

    public bool IsGone => Creature.IsDead || _hasPopped;

    /// <summary>
    ///     Null if this monster still has no pet owner (likely because  <see cref="PlayerCmd.AddPet" /> hasn't been used).
    /// </summary>
    public int? Timestamp { get; private set; }

    internal bool PreventedFromPopping { get; private set; }

    internal int? CurrentSlotIndex { get; private set; }

    internal FigmentSlotMap? CurrentSlotMap { get; private set; }

    internal FigmentSlot? CurrentSlot
    {
        get
        {
            if (CurrentSlotIndex is { } index && CurrentSlotMap is { } map)
                return map.Slots[index];
            return null;
        }
    }

    public override string CustomVisualPath => Pathfinder.Scene("figment");

    internal async Task Initialize(Player owner)
    {
        if (!IsMutable) return;
        _petOwner = owner;

        // Set HP
        var maxHp = MaxHp;
        var additional = owner.Creature.GetPowerAmount<VigorPower>();
        var initial = Math.Min(maxHp, InitialHp + additional);
        await CreatureCmd.SetMaxHp(Creature, maxHp);
        await CreatureCmd.SetCurrentHp(Creature, initial);

        // Find the highest timestamp among all pets (default of 0) and increment it
        var maxTimestamp = owner.Creature.Pets
            .Where(c => c != Creature)
            .Select(c => c.Monster is Figment { Timestamp: { } t } ? t : 0)
            .DefaultIfEmpty(0)
            .Max();
        Timestamp = maxTimestamp + 1;

        // And add pet!
        await PlayerCmd.AddPet(Creature, owner);
    }

    internal void SetSlot(int slotIndex, FigmentSlotMap slotMap, NCreature nOwner, double shiftDuration)
    {
        if (!IsMutable) return;

        var nFigment = Creature.GetCreatureNode();
        if (nFigment is null) return;

        var oldSlotIndex = CurrentSlotIndex ?? slotIndex;
        var oldSlot = slotMap.Slots[oldSlotIndex];
        var oldPos = nOwner.Position + oldSlot.IdlePosition;
        nFigment.Position = oldPos;
        nFigment.ToggleIsInteractable(true);

        var newSlot = slotMap.Slots[slotIndex];
        var newPos = nOwner.Position + newSlot.IdlePosition;
        if (newPos != oldPos) ShiftNode(nFigment, newPos, null, shiftDuration, 0.25, 0.15, true, true);

        var parent = nFigment.GetParent<CanvasItem>();
        parent.YSortEnabled = true;
        nFigment.YSortEnabled = true;

        CurrentSlotIndex = slotIndex;
        CurrentSlotMap = slotMap;
    }

    internal void FaceTarget(Creature target, double duration)
    {
        if (!IsMutable) return;
        if (Creature.GetCreatureNode() is { } nFigment)
            TweenForTurn(nFigment, CreatureIsToLeft(nFigment, target), duration);
    }

    internal void ResetFacing(double duration)
    {
        if (!IsMutable) return;
        if (Creature.GetCreatureNode() is { } nFigment)
            TweenForTurn(nFigment, false, duration);
    }

    private static bool CreatureIsToLeft(NCreature nFigment, Creature creature)
    {
        return creature.GetCreatureNode() is { } nCreature && nFigment.Position.X > nCreature.Position.X;
    }

    private static void TweenForTurn(NCreature nFigment, bool turnLeft, double duration)
    {
        var visuals = nFigment.Visuals;
        var target = turnLeft ? -1f : +1f;
        var easeType = Math.Sign(visuals.Scale.X) == Math.Sign(target) ? Tween.EaseType.Out : Tween.EaseType.InOut;
        var tween = visuals.CreateTween();
        tween
            .TweenProperty(visuals, "scale:x", target, duration)
            .SetEase(easeType)
            .SetTrans(Tween.TransitionType.Expo);
    }

    internal async Task SetMoveAndPowers(PlayerChoiceContext choiceContext)
    {
        if (!IsMutable) return;
        OwnerHpThreshold = Math.Max(PetOwner.Creature.CurrentHp, OwnerHpThreshold);
        SetNextMove();
        await ApplyOwnPowers(choiceContext);
    }

    private protected abstract Task ApplyOwnPowers(PlayerChoiceContext choiceContext);

    public override Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        if (creature != PetOwner.Creature) return Task.CompletedTask;
        OwnerHpThreshold = Math.Max(creature.CurrentHp, OwnerHpThreshold);
        return Task.CompletedTask;
    }

    internal async Task UseAndAdvanceMove(PlayerChoiceContext choiceContext, MoveParams? moveParams,
        TriggerKind trigger)
    {
        if (!IsMutable) return;
        await UseMove(choiceContext, moveParams ?? MoveParams.None, trigger);
        SetNextMove();
    }

    internal async Task UseMove(PlayerChoiceContext choiceContext, MoveParams moveParams, TriggerKind trigger)
    {
        // Can still use move if _markedForPopping
        if (!IsMutable || _hasPopped) return;

        if (CurrentIntent is not { } intent ||
            !intent.CanPerform(this, PetOwner, moveParams, trigger, out var moveTarget))
        {
            if (_markedForPopping) ShiftToDeathbed();
            return;
        }

        MovesUsed += 1;
        CurrentIntent = null; // can't let a move trigger itself again

        await _actionAnimator.FinishAllAnimations();

        if (Creature.GetCreatureNode() is not { } nFigment ||
            PetOwner.Creature.GetCreatureNode() is not { } nOwner)
        {
            await intent.PerformMove(this, PetOwner, moveParams, trigger, choiceContext);
            return;
        }

        var origPos = nFigment.Position;
        var origScale = Vector2.One; // hardcoded for now

        var spotlightPos = CurrentSlot is not null
            ? nOwner.Position + CurrentSlot.SpotlightPosition
            : new Vector2(origPos.X, nOwner.Position.Y);
        const float spotlightMult = 0.6f / 0.5f; // hardcoded for now (matches original scales in Godot scene)
        var spotlightScale = moveTarget is not null && CreatureIsToLeft(nFigment, moveTarget)
            ? new Vector2(-spotlightMult, spotlightMult)
            : new Vector2(+spotlightMult, spotlightMult);

        await intent.BeforePerform(this, PetOwner, moveParams, trigger, choiceContext);
        var shiftDuration = GetShiftDuration();
        ShiftNode(nFigment, spotlightPos, spotlightScale, shiftDuration, 0.10, 0.10, false, false);

        var anim = Skeleton.GetAnimation(intent.GetAnimationId(this, PetOwner, moveParams, trigger));
        _actionAnimator = StandardActionAnimator.Begin(Creature, anim, shiftDuration);
        await _actionAnimator.WaitForActionImpact();
        await intent.PerformMove(this, PetOwner, moveParams, trigger, choiceContext);
        if (IsGone) return; // return immediately if figment killed/popped itself
        await _actionAnimator.WaitForActionEnd();

        if (_markedForPopping)
            ShiftToDeathbed(origScale);
        else
            ShiftNode(nFigment, origPos, origScale, shiftDuration, 0.10, 0.10, false, true);

        await intent.AfterPerform(this, PetOwner, moveParams, trigger, choiceContext);
    }

    private void SetNextMove()
    {
        if (!IsAvailable) return;

        CountMovesRemaining(out var min, out var max);
        if (max < min)
            MainFile.Logger.Warn(
                $"{GetType().Name} returned max < min from {nameof(CountMovesRemaining)}.");

        MinRemainingMoves = min;
        MaxRemainingMoves = max < min ? null : max;
        if (MaxRemainingMoves == 0 || GetNextFigmentIntent() is not { } intent)
        {
            if (MinRemainingMoves > 0)
                MainFile.Logger.Warn(
                    $"{GetType().Name} returned min > 0 from {nameof(CountMovesRemaining)}, but could not get intent.");

            CurrentIntent = null;
            SetMoveImmediate(new MoveState("EMPTY_MOVE", _ => Task.CompletedTask));
            MainFile.Logger.Info($"{GetType().Name} has no more moves to intend.");
            return;
        }

        CurrentIntent = intent;
        SetMoveImmediate(new MoveState("INTENT_ONLY_MOVE", _ => Task.CompletedTask, intent));
        MainFile.Logger.Info($"{GetType().Name} intends to use {intent.IntentType} move.");
    }

    protected abstract void CountMovesRemaining(out uint min, out uint? max);

    protected abstract FigmentIntent? GetNextFigmentIntent();

    protected sealed override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        var state = new MoveState("BLANK_MOVE", _ => Task.CompletedTask);
        state.FollowUpState = state;
        return new MonsterMoveStateMachine([state], state);
    }

    public override CreatureAnimator GenerateAnimator(MegaSprite controller)
    {
        Skeleton.LoadIntoSprite(controller);

        CreatureAnimator creatureAnimator;
        var idleState = new AnimState(Skeleton.IdleId, true);
        if (Skeleton.StartId is { } startId)
        {
            var startState = new AnimState(startId) { NextState = idleState };
            creatureAnimator = new CreatureAnimator(startState, controller);
        }
        else
        {
            creatureAnimator = new CreatureAnimator(idleState, controller);
        }

        creatureAnimator.AddAnyState(CreatureAnimator.idleTrigger, idleState);
        creatureAnimator.AddAnyState(CreatureAnimator.deathTrigger, new AnimState(Skeleton.DieId));
        return creatureAnimator;
    }

    /// <summary>
    ///     Claim responsibility for calling <see cref="Pop" /> at some point before ceding control back to STS2.
    ///     This is necessary for some callers that plan on popping the figment but not immediately.
    /// </summary>
    internal void MarkForPopping()
    {
        if (!IsMutable) return;
        _markedForPopping = true;
    }

    protected void PreventFromPopping()
    {
        if (!IsMutable) return;
        PreventedFromPopping = true;
    }

    internal async Task Pop()
    {
        var creature = Creature;
        if (!IsMutable || _hasPopped || creature.IsDead ||
            creature.CombatState is not { } combatState || !combatState.IsLiveCombat())
            return;

        _markedForPopping = true;
        _hasPopped = true;
        if (NCombatRoom.Instance?.GetCreatureNode(creature) is { } node)
        {
            await Task.WhenAll(
                _actionAnimator.FinishAllAnimations(),
                _tweenTimer?.WaitForTimeout() ?? Task.CompletedTask);
            node.StartDeathAnim(true);
            NCombatRoom.Instance.RemoveCreatureNode(node);
        }

        foreach (var power in creature.RemoveAllPowersAfterDeath()) await power.AfterRemoved(creature);
        CombatManager.Instance.RemoveCreature(creature);

        // NOTE: this is an open design question. Of the three (?) options (report death, report escape, report nothing),
        // doesn't escape make the most sense...?
        combatState.CreatureEscaped(creature);
    }

    internal void ShiftToDeathbed(Vector2? scale = null)
    {
        if (Creature.GetCreatureNode() is not { } nFigment ||
            PetOwner.Creature.GetCreatureNode() is not { } nOwner ||
            CurrentSlot is not { } slot)
            return;

        var deathbed = nOwner.Position + slot.GetRandomDeathbedPosition();
        var shiftDuration = GetShiftDuration();
        ShiftNode(nFigment, deathbed, scale, shiftDuration, 0.10, 0.10, false, false);
    }

    private static float GetShiftDuration()
    {
        return SaveManager.Instance.PrefsSave.FastMode switch
        {
            FastModeType.None => 0,
            FastModeType.Normal => 0.35f,
            FastModeType.Fast => 0.25f,
            FastModeType.Instant => 0,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void ShiftNode(
        NCreature node,
        Vector2 newPos,
        Vector2? newScale,
        double shiftDuration,
        double fadeOutOrInDuration,
        double fadeInDelay,
        bool obscureVisuals,
        bool fadeUiBackIn)
    {
        const Tween.EaseType shiftEase = Tween.EaseType.InOut;
        const Tween.TransitionType shiftTrans = Tween.TransitionType.Cubic;

        node.CreateTween()
            .TweenProperty(node, "position", newPos, shiftDuration)
            .SetEase(shiftEase)
            .SetTrans(shiftTrans);

        TweenForFade(new FadeTweenParams(node.Visuals)
        {
            OutColor = obscureVisuals ? Colors.Black : null,
            InColor = obscureVisuals ? Colors.White : null,
            OutOrInDuration = fadeOutOrInDuration,
            InDelay = fadeInDelay,
            NewScale = newScale,
            ScaleDuration = shiftDuration,
            ScaleEase = shiftEase,
            ScaleTrans = shiftTrans
        });

        TweenForFade(new FadeTweenParams(node.IntentContainer)
        {
            OutColor = Colors.Transparent,
            InColor = fadeUiBackIn ? Colors.White : Colors.Transparent,
            OutOrInDuration = fadeOutOrInDuration,
            InDelay = fadeInDelay
        });

        if (node.GetChildren().FirstOrDefault(n => n is NCreatureStateDisplay) is { } nDisplay)
            TweenForFade(new FadeTweenParams(nDisplay)
            {
                OutColor = Colors.Transparent,
                InColor = fadeUiBackIn ? Colors.White : Colors.Transparent,
                OutOrInDuration = fadeOutOrInDuration,
                InDelay = fadeInDelay
            });

        if (_tweenTimer is not null && _tweenTimer.TimerRunning)
            MainFile.Logger.Info(
                $"Figment {this} has an ongoing tween timer. Replacing it (this may or may not have a visual impact).");

        _tweenTimer = Timer.Start(shiftDuration);
    }

    private static void TweenForFade(FadeTweenParams p)
    {
        if (p.OutColor is null && p.NewScale is null && p.InColor is null) return;

        var tween = p.Node.CreateTween();

        if (p.OutColor is { } outColor)
            tween
                .TweenProperty(p.Node, "modulate", outColor, p.OutOrInDuration)
                .SetEase(Tween.EaseType.In)
                .SetTrans(Tween.TransitionType.Quad);

        if (p.NewScale is { } newScale)
            tween.Parallel()
                .TweenProperty(p.Node, "scale", newScale, p.ScaleDuration)
                .SetEase(p.ScaleEase)
                .SetTrans(p.ScaleTrans);

        if (p.InColor is { } inColor)
            tween.Chain()
                .TweenProperty(p.Node, "modulate", inColor, p.OutOrInDuration)
                .SetDelay(p.InDelay ?? 0)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Quad);
    }

    private class FadeTweenParams(Node node)
    {
        public Node Node { get; } = node;
        public Color? OutColor { get; init; }
        public double OutOrInDuration { get; init; }
        public Vector2? NewScale { get; init; }
        public double ScaleDuration { get; init; }
        public Tween.EaseType ScaleEase { get; init; } = Tween.EaseType.InOut;
        public Tween.TransitionType ScaleTrans { get; init; } = Tween.TransitionType.Linear;
        public double? InDelay { get; init; }
        public Color? InColor { get; init; }
    }
}

public abstract class Figment<TDefense, TLifecycle, TTalent> : Figment
    where TDefense : FigmentDefensePower
    where TLifecycle : FigmentLifecyclePower
    where TTalent : FigmentTalentPower
{
    private protected sealed override async Task ApplyOwnPowers(PlayerChoiceContext choiceContext)
    {
        var creature = Creature;
        Creature? applier = null; // same behavior as Osty, but this is open to change if it makes sense
        CardModel? cardSource = null; // ditto
        await PowerCmd.Apply<TTalent>(
            choiceContext, creature, InitialTalentPowerAmount, applier, cardSource, true);
        await PowerCmd.Apply<TDefense>(
            choiceContext, creature, 1, applier, cardSource, true);
        var lifecycle = await PowerCmd.Apply<TLifecycle>(
            choiceContext, creature, InitialLifecyclePowerAmount, applier, cardSource, true);
        if (lifecycle?.PreventsPopping ?? false) PreventFromPopping();
    }
}