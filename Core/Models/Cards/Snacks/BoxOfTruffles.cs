using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Commands;
using Trimaw.Core.Models.Powers.Talents;
using Trimaw.Core.Utils;

namespace Trimaw.Core.Models.Cards.Snacks;

public class BoxOfTruffles : SnackCard
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipHelper
        .ForImagineRandom<ArtsAssimilationTalent>(this)
        .Concat([HoverTipFactory.Static(StaticHoverTip.SuperSpecial)]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ImaginationCmd.ImagineRandom<ArtsAssimilationTalent>(choiceContext, Owner);

        IEnumerable<CardModel> source = PileType.Hand.GetPile(Owner).Cards;
        if (IsUpgraded) source = source.Concat(PileType.Draw.GetPile(Owner).Cards);
        var cards = source.Where(c => c.Type == CardType.Skill).ToArray();
        if (cards.Length < 1) return;

        var specialCards = cards.Select(c => c.CreateClone()).ToArray();
        foreach (var card in specialCards) await TrimawCmd.MakeSuperSpecial(card);

        var transforms = cards.Zip(specialCards, (a, b) => new CardTransformation(a, b));
        await CardCmd.Transform(transforms, null);
    }
}