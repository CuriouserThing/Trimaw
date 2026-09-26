using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.Triggers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Monsters.Figments;

public class BigUglyThingFigment : Figment<ImaginaryShieldPower, BigUglyThingTrigger, PlaceholderTalent>
{
    private const int MoveCount = 5;

    private bool _isInBirdPhase;
    public static string AttackAId => "Attack_1";
    public static string AttackBId => "Attack_2_Up";
    public static string AttackCId => "Skill";
    public static string ExplosionId => "Revive";
    public static string SummonId => "Move_2";

    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_1512_mcmstr",
            new ActionAnimation(AttackAId, 0.25, 0.60, 1.00, "Idle_1"))
        .WithSecondaryAnimation(new ActionAnimation(AttackBId, 0.20, 0.70, 1.20, "Idle_1"))
        .WithSecondaryAnimation(new ActionAnimation(AttackCId, 0.50, 1.58, 2.10, "Idle_1"))
        .WithSecondaryAnimation(new ActionAnimation(ExplosionId, 1.00, 2.06, 3.00, "Idle_2") { Timescale = 1.5 })
        .WithSecondaryAnimation(new ActionAnimation(SummonId, 0.50, 0.50, 0.60, "Idle_2") { CutoffTime = 1.10 });

    public override string CustomVisualPath => Pathfinder.Scene("big_ugly_thing");

    protected override int InitialHp => 23;
    protected override int MaxHp => 28;

    // Pretend the explosion damaged the High Priest too for flavor :)
    private static int InitialBirdHp => 3;
    private static int MaxBirdHp => InitialBirdHp + (int)BigUglyThingExplosionIntent.ExplosionDamage;

    protected override int InitialTriggerPowerAmount => MoveCount - 1;

    protected override async Task<FigmentIntent?> ReadyNextIntent(PlayerChoiceContext choiceContext)
    {
        switch (MovesUsed)
        {
            case 0:
                return new BigUglyThingAttackAIntent();
            case 1:
                return new BigUglyThingAttackBIntent();
            case 2:
                return new BigUglyThingAttackCIntent();
            case 3:
                return new BigUglyThingExplosionIntent();
            case 4:
                await ChangeToBirdPhase(choiceContext);
                return new BigUglyThingSummonIntent();
            default:
                return null;
        }
    }

    private async Task ChangeToBirdPhase(PlayerChoiceContext choiceContext)
    {
        if (_isInBirdPhase)
        {
            MainFile.Logger.Warn("The Big Ugly Thing is already in its High Priest phase; cannot change again.");
            return;
        }

        _isInBirdPhase = true;
        await CreatureCmd.SetCurrentHp(Creature, InitialBirdHp);
        await CreatureCmd.SetMaxHp(Creature, MaxBirdHp);

        // Move intent down to this shorty's new height
        if (Creature.GetCreatureNode() is { } node)
        {
            var pos = node.IntentContainer.GetPosition();
            node.IntentContainer.SetPosition(new Vector2(pos.X, pos.Y + 180));
        }

        await PowerCmd.Apply<SkillTrigger>(choiceContext, Creature, 2, Creature, null, true);
        await PowerCmd.Apply<ImaginaryShieldPower>(choiceContext, Creature, 1, Creature, null, true);
    }

    public bool IsInMechPhase()
    {
        return !_isInBirdPhase;
    }

    public bool IsInBirdPhase()
    {
        return _isInBirdPhase;
    }

    public override CreatureAnimator GenerateAnimator(MegaSprite controller)
    {
        Skeleton.LoadIntoSprite(controller);

        var mechIdleState = new AnimState("Idle_1", true);
        var mechDieState = new AnimState("Stun_1");
        var birdIdleState = new AnimState("Idle_2", true);
        var birdDieState = new AnimState("Die");

        var creatureAnimator = new CreatureAnimator(mechIdleState, controller);
        creatureAnimator.AddAnyState(CreatureAnimator.idleTrigger, mechIdleState, IsInMechPhase);
        creatureAnimator.AddAnyState(CreatureAnimator.deathTrigger, mechDieState, IsInMechPhase);
        creatureAnimator.AddAnyState(CreatureAnimator.idleTrigger, birdIdleState, IsInBirdPhase);
        creatureAnimator.AddAnyState(CreatureAnimator.deathTrigger, birdDieState, IsInBirdPhase);
        return creatureAnimator;
    }
}