using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class TomimiIntent : LabeledFigmentIntent<TomimiFigment>
{
    private const decimal Damage = 6;

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Defend;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("armadillo_tail");

    protected override void FormatIntentLabel(LocString label, MoveContext<TomimiFigment> ctx)
    {
        var damage = ctx.TransformAmount(Damage);
        FormatWithAnyCreatureDamage(label, ctx, new DamageVar(damage, ValueProp.Move));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<TomimiFigment> ctx)
    {
        desc.Add(new DamageVar(Damage, ValueProp.Move));
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<TomimiFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        var cmd = await DamageCmd
            .Attack(ctx.TransformAmount(Damage))
            .FromFigment(ctx.MoveUser)
            .TargetingRandomOpponents(ctx.CombatState)
            .Execute(choiceCtx);
        var damage = cmd.Results.SelectMany(l => l).Sum(r => r.TotalDamage + r.OverkillDamage);
        await CreatureCmd.GainBlock(ctx.PetOwner.Creature, damage, ValueProp.Move, null);
        return FigmentMoveResult.Success;
    }
}