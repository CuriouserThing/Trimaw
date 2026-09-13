using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Trimaw.Core.Commands;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Relics;

public class Lunchbox : TrimawRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override string Icon85Path => Pathfinder.NotoEmoji85("bento_box");
    public override string Icon256Path => Pathfinder.NotoEmoji256("bento_box");

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        await SnackCmd.PrepRandom(choiceContext, combatState, player);
    }
}