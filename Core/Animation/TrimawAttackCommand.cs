using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Trimaw.Core.Animation;

public class TrimawAttackCommand
{
    private readonly CardModel _card;
    private readonly CardPlay? _cardPlay;
    private readonly decimal _damagePerHit;
    private CeobeAttack? _animation;
    private ICombatState? _combatState;
    private bool _doesRandomTargetingAllowDuplicates;
    private bool _executed;
    private int? _hitCount;
    private bool _isRandomlyTargeted;
    private bool _onlyPlayAnimOnce;
    private Creature? _singleTarget;
    private bool _targetsSet;
    private float? _timescale;

    internal TrimawAttackCommand(CardModel card, CardPlay? cardPlay, decimal damagePerHit)
    {
        _card = card;
        _cardPlay = cardPlay;
        _damagePerHit = damagePerHit;
    }

    public TrimawAttackCommand WithHitCount(int hitCount)
    {
        if (_hitCount is not null)
            throw new InvalidOperationException(
                $"Called {nameof(WithHitCount)} twice while building this command.");
        _hitCount = hitCount;
        return this;
    }

    public TrimawAttackCommand Targeting(Creature target)
    {
        ThrowIfTargetsSet();
        _singleTarget = target;
        return this;
    }

    public TrimawAttackCommand TargetingAllOpponents(ICombatState combatState)
    {
        ThrowIfTargetsSet();
        _combatState = combatState;
        return this;
    }

    public TrimawAttackCommand TargetingRandomOpponents(ICombatState combatState, bool allowDuplicates = true)
    {
        ThrowIfTargetsSet();
        _combatState = combatState;
        _isRandomlyTargeted = true;
        _doesRandomTargetingAllowDuplicates = allowDuplicates;
        return this;
    }

    private void ThrowIfTargetsSet()
    {
        if (_targetsSet)
            throw new InvalidOperationException(
                $"Targets already set on this {nameof(TrimawAttackCommand)}. Only once call {nameof(Targeting)}, {nameof(TargetingAllOpponents)}, or {nameof(TargetingRandomOpponents)}.");
        _targetsSet = true;
    }

    public TrimawAttackCommand WithStaffAnimation(bool onlyPlayOnce = false)
    {
        SetAnimation(CeobeAttack.Staff);
        _onlyPlayAnimOnce = onlyPlayOnce;
        return this;
    }

    public TrimawAttackCommand WithAxeAnimation(bool onlyPlayOnce = false)
    {
        SetAnimation(CeobeAttack.Axe);
        _onlyPlayAnimOnce = onlyPlayOnce;
        return this;
    }

    public TrimawAttackCommand WithKnifeAnimation(bool onlyPlayOnce = false)
    {
        SetAnimation(CeobeAttack.Knife);
        _onlyPlayAnimOnce = onlyPlayOnce;
        return this;
    }

    public TrimawAttackCommand WithSpearAnimation(bool onlyPlayOnce = false)
    {
        SetAnimation(CeobeAttack.Spear);
        _onlyPlayAnimOnce = onlyPlayOnce;
        return this;
    }

    private void SetAnimation(CeobeAttack animation)
    {
        if (_animation is not null)
            throw new InvalidOperationException(
                $"Called {nameof(WithStaffAnimation)}, {nameof(WithAxeAnimation)}, {nameof(WithKnifeAnimation)}, or {nameof(WithSpearAnimation)} twice while building this command.");
        if (!Enum.IsDefined(animation))
            throw new ArgumentOutOfRangeException(nameof(animation));
        _animation = animation;
    }

    public TrimawAttackCommand WithTimescale(float timescale)
    {
        if (_timescale is not null)
            throw new InvalidOperationException(
                $"Called {nameof(WithTimescale)} twice while building this command.");
        _timescale = timescale;
        return this;
    }

    public async Task<AttackCommand> Execute(PlayerChoiceContext? choiceContext)
    {
        if (_executed) throw new InvalidOperationException("Attempted to execute attack command twice.");
        _executed = true;

        var hitCount = _hitCount ?? 1;
        var cmd = DamageCmd
            .Attack(_damagePerHit)
            .WithHitCount(hitCount)
            .FromCard(_card, _cardPlay);
        cmd = WithTargets(cmd);
        cmd = WithFx(cmd);

        if (_onlyPlayAnimOnce)
            cmd = cmd.OnlyPlayAnimOnce();

        if (_card.Owner.Character is Trimaw)
            cmd = cmd.WithNoAttackerAnim().AfterAttackerAnim(Animate);

        return await cmd.Execute(choiceContext);
    }

    private async Task Animate()
    {
        if (_animation is not { } anim ||
            _card.Owner.Creature.GetCreatureNode()?.Visuals.SpineBody?.GetAnimationState() is not { } animState)
            return;

        var result = await MainFile.CombatManagerFactory.GetOrCreate(_card.Owner)
            .AnimateAttack(animState, anim, 1, true, _timescale ?? 1f);
        if (result.TimeBeforeFirstHit is { } wait)
            await Cmd.Wait(wait);
    }

    private AttackCommand WithTargets(AttackCommand cmd)
    {
        if (_combatState is null)
        {
            if (_singleTarget is null)
                throw new InvalidOperationException(
                    $"No target set for {nameof(TrimawAttackCommand)}. Call either {nameof(Targeting)}, {nameof(TargetingAllOpponents)}, {nameof(TargetingRandomOpponents)}.");

            return cmd.Targeting(_singleTarget);
        }

        if (_isRandomlyTargeted) return cmd.TargetingRandomOpponents(_combatState, _doesRandomTargetingAllowDuplicates);

        return cmd.TargetingAllOpponents(_combatState);
    }

    private AttackCommand WithFx(AttackCommand cmd)
    {
        return _animation switch
        {
            CeobeAttack.Staff => cmd.WithHitFx("vfx/vfx_attack_slash"),
            CeobeAttack.Axe => cmd.WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3"),
            CeobeAttack.Knife => cmd.WithHitFx("vfx/vfx_dramatic_stab", tmpSfx: "dagger_throw.mp3"),
            CeobeAttack.Spear => cmd.WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3"),
            _ => cmd
        };
    }
}