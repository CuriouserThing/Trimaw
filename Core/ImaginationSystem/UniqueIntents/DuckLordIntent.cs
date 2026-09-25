using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using Trimaw.Core.CombatHistory;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.Models.Powers.CeobePowers;
using Trimaw.Core.Utils;

namespace Trimaw.Core.ImaginationSystem.UniqueIntents;

public class DuckLordIntent : LabeledFigmentIntent<DuckLordFigment>
{
    private static readonly Func<MoveContext, PlayerChoiceContext, double, Task>[] Actions =
        [StDamage, AoeDamage, VigorBuff, AimBuff];

    private protected override VanillaIntentWrapper DefaultVanillaIntent => VanillaIntentWrapper.Unknown;

    protected override string DefaultTipIconPath => Pathfinder.NotoEmoji64("duck");

    private static int GetAmount(MoveContext ctx)
    {
        return MainFile.CombatManagerFactory.GetOrCreate(ctx.PetOwner).AllHistoryEntries
            .OfType<GoldSpentOnFigmentEntry>()
            .Sum(e => e.GoldAmount);
    }

    protected override void FormatIntentLabel(LocString label, MoveContext<DuckLordFigment> ctx)
    {
        label.Add(new RepeatVar(GetAmount(ctx)));
    }

    protected override bool CanPerform(MoveContext<DuckLordFigment> ctx, out Creature? visualTarget)
    {
        visualTarget = null;
        return GetAmount(ctx) > 0;
    }

    protected override async Task<FigmentMoveResult> OnPerform(MoveContext<DuckLordFigment> ctx,
        PlayerChoiceContext choiceCtx)
    {
        var startingFlipDuration = GetStartingDuration();
        const double minFlipDuration = 0.05;
        const double flipDurationAccel = 0.95;
        var totalGold = GetAmount(ctx);
        for (var i = 0; i < totalGold; i += 1)
        {
            var action = ctx.MoveUser.RunRng.MonsterAi.NextItem(Actions);
            if (action is null) continue;
            var duration = Math.Max(minFlipDuration, startingFlipDuration * Math.Pow(flipDurationAccel, i));
            await action(ctx, choiceCtx, duration);
        }

        return FigmentMoveResult.Success;
    }

    private static async Task StDamage(MoveContext ctx, PlayerChoiceContext choiceCtx, double duration)
    {
        ctx.MoveUser.ResetFacing(duration);
        await Cmd.Wait((float)duration);
        var x = ctx.MoveUser.RunRng.MonsterAi.NextGaussianInt(300, 80, 100, 1000);
        await DamageCmd
            .Attack(x / 100M)
            .FromFigment(ctx.MoveUser)
            .TargetingRandomOpponents(ctx.CombatState)
            .Execute(choiceCtx);
    }

    private static async Task AoeDamage(MoveContext ctx, PlayerChoiceContext choiceCtx, double duration)
    {
        ctx.MoveUser.ResetFacing(duration);
        await Cmd.Wait((float)duration);
        var x = ctx.MoveUser.RunRng.MonsterAi.NextGaussianInt(200, 60, 100, 500);
        await DamageCmd
            .Attack(x / 100M)
            .FromFigment(ctx.MoveUser)
            .TargetingAllOpponents(ctx.CombatState)
            .Execute(choiceCtx);
    }

    private static async Task VigorBuff(MoveContext ctx, PlayerChoiceContext choiceCtx, double duration)
    {
        var target = ctx.PetOwner.Creature;
        ctx.MoveUser.FaceTarget(target, duration);
        await Cmd.Wait((float)duration);
        await PowerCmd.Apply<VigorPower>(choiceCtx, target, 1, ctx.MoveUser.Creature, null);
    }

    private static async Task AimBuff(MoveContext ctx, PlayerChoiceContext choiceCtx, double duration)
    {
        var target = ctx.PetOwner.Creature;
        ctx.MoveUser.FaceTarget(target, duration);
        await Cmd.Wait((float)duration);
        await PowerCmd.Apply<AimPower>(choiceCtx, target, 1, ctx.MoveUser.Creature, null);
    }

    private static double GetStartingDuration()
    {
        return SaveManager.Instance.PrefsSave.FastMode switch
        {
            FastModeType.None => 0,
            FastModeType.Normal => 0.50,
            FastModeType.Fast => 0.40,
            FastModeType.Instant => 0,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}