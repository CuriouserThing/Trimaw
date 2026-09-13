using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Models.Powers;
using Trimaw.Core.Models.Powers.SharedTalents;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class BigUglyThingExplosionIntent : UnlabeledFigmentIntent<BigUglyThingFigment>
{
    private static readonly DamageVar Damage = new(4, ValueProp.Move);
    private static readonly DynamicVar Frail = new PowerVar<FrailPower>(2);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.DeathBlow;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("cogsplosion");

    protected override void FormatTipDescription(LocString desc, MoveContext<BigUglyThingFigment> ctx)
    {
        desc.Add(Damage);
        desc.Add(Frail);
    }

    protected override string GetAnimationId(MoveContext<BigUglyThingFigment> ctx)
    {
        return BigUglyThingFigment.ExplosionId;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<BigUglyThingFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        var creature = ctx.MoveUser.Creature;
        var explosionTargets = ctx.CombatState.HittableEnemies.Concat(ctx.CombatState.PlayerCreatures).ToArray();
        await CreatureCmd.Damage(choiceCtx, explosionTargets, Damage, creature, null, null);
        await PowerCmd.Apply<FrailPower>(choiceCtx, explosionTargets, Frail.BaseValue, creature, null);

        foreach (var power in creature.Powers.ToArray()) await PowerCmd.Remove(power);

        // Pretend the explosion damaged the High Priest too for flavor :)
        var initialHp = ctx.MoveUser.InitialBirdHp;
        await CreatureCmd.SetCurrentHp(creature, initialHp);
        await CreatureCmd.SetMaxHp(creature, initialHp + Damage.BaseValue);
        ctx.MoveUser.ChangeToBirdPhase();

        await PowerCmd.Apply<SkillTrigger>(choiceCtx, creature, 2, creature, null, true);
        await PowerCmd.Apply<ImaginaryShieldPower>(choiceCtx, creature, 1, creature, null, true);
        await PowerCmd.Apply<EvanescentPower>(choiceCtx, creature, 1, creature, null, true);

        return FigmentMoveResult.Success;
    }
}