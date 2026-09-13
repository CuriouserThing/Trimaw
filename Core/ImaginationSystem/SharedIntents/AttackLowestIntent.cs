using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.SharedIntents;

public class AttackLowestIntent(int amount) : LabeledFigmentIntent<Figment>
{
    private readonly DamageVar _damage = new(amount, ValueProp.Move);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack2;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("dagger_knife");

    private static Creature? GetTarget(MoveContext ctx)
    {
        return ctx.CombatState.HittableEnemies.MinBy(c => c.CurrentHp);
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<Figment> ctx)
    {
        FormatWithTargetedDamage(label, ctx, GetTarget(ctx), _damage);
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<Figment> ctx)
    {
        FormatWithTargetedDamage(desc, ctx, GetTarget(ctx), _damage);
    }

    protected override bool CanPerform(MoveContext<Figment> ctx, out Creature? target)
    {
        target = GetTarget(ctx);
        return target is not null;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<Figment> ctx, PlayerChoiceContext choiceCtx)
    {
        if (GetTarget(ctx) is { } target)
            await DamageCmd
                .Attack(_damage.BaseValue)
                .FromFigment(ctx.MoveUser)
                .Targeting(target)
                .Execute(choiceCtx);

        return FigmentMoveResult.Success;
    }
}