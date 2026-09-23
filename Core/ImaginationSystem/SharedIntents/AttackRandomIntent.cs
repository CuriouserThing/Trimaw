using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.SharedIntents;

public class AttackRandomIntent(int amount) : LabeledFigmentIntent<Figment>
{
    private readonly DamageVar _damage = new(amount, ValueProp.Move);

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Attack3;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("collision");

    protected override void FormatIntentLabel(LocString label, MoveContext<Figment> ctx)
    {
        FormatWithAnyCreatureDamage(label, ctx, _damage);
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<Figment> ctx)
    {
        FormatWithAnyCreatureDamage(desc, ctx, _damage);
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<Figment> ctx, PlayerChoiceContext choiceCtx)
    {
        await DamageCmd
            .Attack(_damage.BaseValue)
            .FromFigment(ctx.MoveUser)
            .TargetingRandomOpponents(ctx.CombatState)
            .Execute(choiceCtx);

        return FigmentMoveResult.Success;
    }
}