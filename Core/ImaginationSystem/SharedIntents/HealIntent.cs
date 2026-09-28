using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.SharedIntents;

public class HealIntent(int amount) : LabeledFigmentIntent<Figment>
{
    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Heal;

    protected override string DefaultTipIconPath => Pathfinder.VanillaPower64<RegenPower>();

    protected override void FormatIntentLabel(LocString label, MoveContext<Figment> ctx)
    {
        label.Add(new HealVar(ctx.TransformAmount(amount)));
    }

    protected override void FormatTipDescription(LocString desc, MoveContext<Figment> ctx)
    {
        desc.Add(new HealVar(amount));
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<Figment> ctx, PlayerChoiceContext choiceCtx)
    {
        var heal = ctx.TransformAmount(amount);
        foreach (var figment in ctx.PetOwner.GetFigments())
        {
            var target = figment.Creature;
            var cappedHeal = Math.Min(heal, target.MaxHp - target.CurrentHp);
            var overHeal = heal - cappedHeal;
            await CreatureCmd.Heal(target, cappedHeal);
            if (overHeal > 0)
                await CreatureCmd.Damage(choiceCtx, ctx.CombatState.HittableEnemies, overHeal, ValueProp.Unpowered,
                    ctx.MoveUser.Creature);
        }

        return FigmentMoveResult.Success;
    }
}