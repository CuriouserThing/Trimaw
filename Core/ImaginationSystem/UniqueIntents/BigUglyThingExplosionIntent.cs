using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class BigUglyThingExplosionIntent : UnlabeledFigmentIntent<BigUglyThingFigment>
{
    private const decimal Damage = 4;
    private const decimal Frail = 2;

    public static decimal ExplosionDamage => Damage;

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.DeathBlow;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("cogsplosion");

    protected override void FormatTipDescription(LocString desc, MoveContext<BigUglyThingFigment> ctx)
    {
        desc.Add(new DamageVar(Damage, ValueProp.Unpowered | ValueProp.Move));
        desc.Add(new PowerVar<FrailPower>(Frail));
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
        var damage = new DamageVar(ctx.TransformAmount(Damage), ValueProp.Unpowered | ValueProp.Move);
        await CreatureCmd.Damage(choiceCtx, explosionTargets, damage, creature, null, null);
        await PowerCmd.Apply<FrailPower>(choiceCtx, explosionTargets, ctx.TransformAmount(Frail), creature, null);

        foreach (var power in creature.Powers.ToArray()) await PowerCmd.Remove(power);
        return FigmentMoveResult.Success;
    }
}