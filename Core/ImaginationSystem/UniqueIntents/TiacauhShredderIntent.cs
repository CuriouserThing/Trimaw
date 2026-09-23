using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class TiacauhShredderIntent : LabeledFigmentIntent<TiacauhShredderFigment>
{
    private static readonly DamageVar Damage = new(7, ValueProp.Move);
    private static readonly DamageVar ExtraDamage = new("ExtraDamage", 5, ValueProp.Move);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack3;

    protected override string DefaultTipIconPath => Pathfinder.GameIconsDotnet64("cleaver");

    private static bool TargetReceivesExtra(Creature target)
    {
        return target.HasPower<VulnerablePower>() || target.HasPower<FrailPower>();
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<TiacauhShredderFigment> ctx)
    {
        var damage = ctx.CombatState.HittableEnemies.All(TargetReceivesExtra)
            ? new DamageVar(Damage.BaseValue + ExtraDamage.BaseValue, ValueProp.Move)
            : Damage;
        FormatWithAnyCreatureDamage(label, ctx, damage);
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<TiacauhShredderFigment> ctx)
    {
        desc.Add(Damage);
        desc.Add(ExtraDamage);
    }

    protected override bool CanPerform(MoveContext<TiacauhShredderFigment> ctx, out Creature? target)
    {
        target = ctx.GetTarget();
        return target is not null;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<TiacauhShredderFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        if (ctx.GetTarget() is not { } target) return FigmentMoveResult.NoValidTarget;

        var damage = Damage.BaseValue;
        if (TargetReceivesExtra(target)) damage += ExtraDamage.BaseValue;
        await DamageCmd
            .Attack(damage)
            .FromFigment(ctx.MoveUser)
            .Targeting(target)
            .Execute(choiceCtx);
        return FigmentMoveResult.Success;
    }
}