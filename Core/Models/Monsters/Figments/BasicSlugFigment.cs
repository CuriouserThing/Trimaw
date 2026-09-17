using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.ImaginationSystem.UniqueIntents;
using Trimaw.Core.Models.Powers.SharedTalents;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Monsters.Figments;

public class BasicSlugFigment : SimpleFigment<PrepTrigger, BasicSlugIntent>
{
    private bool _isFourLegged;

    protected override AkCombatSkeleton Skeleton => AkCombatSkeleton.Build("enemy_10001_trslim",
        new ActionAnimation("Attack_B", 0.60, "Idle_B")
        {
            StartId = "Skill_Begin",
            StartKey = 1.00,
            EndId = "Move_B",
            EndKey = 0,
            Timescale = 1.5
        });

    public override string CustomVisualPath => Pathfinder.Scene("sluggy");

    public override HashSet<HardTag> HardTags =>
    [
        HardTag.FromArknights,
        HardTag.UsesAkEnemy,
        HardTag.Slug
    ];

    protected override int InitialHp => 3;
    protected override int MaxHp => 5;
    
    public void ExtendLegs()
    {
        _isFourLegged = true;
    }

    public bool IsZeroLegged()
    {
        return !_isFourLegged;
    }

    public bool IsFourLegged()
    {
        return _isFourLegged;
    }

    public override CreatureAnimator GenerateAnimator(MegaSprite controller)
    {
        Skeleton.LoadIntoSprite(controller);

        var zeroLeggedIdleState = new AnimState("Idle_A", true);
        var zeroLeggedDieState = new AnimState("Die_B");
        var fourLeggedIdleState = new AnimState("Idle_2", true);
        var fourLeggedDieState = new AnimState("Die_B");

        var creatureAnimator = new CreatureAnimator(zeroLeggedIdleState, controller);
        creatureAnimator.AddAnyState(CreatureAnimator.idleTrigger, zeroLeggedIdleState, IsZeroLegged);
        creatureAnimator.AddAnyState(CreatureAnimator.deathTrigger, zeroLeggedDieState, IsZeroLegged);
        creatureAnimator.AddAnyState(CreatureAnimator.idleTrigger, fourLeggedIdleState, IsFourLegged);
        creatureAnimator.AddAnyState(CreatureAnimator.deathTrigger, fourLeggedDieState, IsFourLegged);
        return creatureAnimator;
    }
}