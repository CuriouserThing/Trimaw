using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Models;
using Trimaw.Core.Models.Cards.Basic;
using Trimaw.Core.Models.Relics;
using Trimaw.Core.Utils;

namespace Trimaw.Core;

public sealed class Trimaw : PlaceholderCharacterModel
{
    public const string CharacterId = "Trimaw";
    public override CharacterGender Gender => CharacterGender.Feminine;

    public static Color LabOutlineColor => Color.Color8(0xea, 0xa7, 0x39); // bright earth
    public override Color MapDrawingColor => Color.Color8(0x99, 0x33, 0xcc); // Cerberus/berry purple
    public override Color NameColor => LabOutlineColor;

    public override int StartingHp => 70;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeTrimaw>(),
        ModelDb.Card<StrikeTrimaw>(),
        ModelDb.Card<StrikeTrimaw>(),
        ModelDb.Card<StrikeTrimaw>(),
        ModelDb.Card<DefendTrimaw>(),
        ModelDb.Card<DefendTrimaw>(),
        ModelDb.Card<DefendTrimaw>(),
        ModelDb.Card<DefendTrimaw>(),
        ModelDb.Card<DoubleAxe>(),
        ModelDb.Card<Scrounge>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<Lunchbox>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<TrimawCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<TrimawRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<TrimawPotionPool>();

    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomVisualPath => Pathfinder.Scene("ceobe");
    public override string CustomCharacterSelectBg => Pathfinder.Scene("char_select_bg_trimaw");

    public override string CustomCharacterSelectIconPath => Pathfinder.UiImage("char_select_trimaw");
    public override string CustomCharacterSelectLockedIconPath => Pathfinder.UiImage("char_select_trimaw_locked");
    public override string CustomIconTexturePath => Pathfinder.UiImage("char_icon_trimaw");
    public override string CustomIconOutlineTexturePath => Pathfinder.UiImage("char_icon_trimaw_outline");
    public override string CustomMapMarkerPath => Pathfinder.UiImage("map_marker_trimaw");

    public override CreatureAnimator GenerateAnimator(MegaSprite controller, Creature creature)
    {
        var idleState = new AnimState("Idle", true);
        var startState = new AnimState("Start") { NextState = idleState };

        var creatureAnimator = new CreatureAnimator(startState, controller);
        creatureAnimator.AddAnyState(CreatureAnimator.idleTrigger, idleState);

        creatureAnimator.AddAnyState(CreatureAnimator.attackTrigger, new AnimState("Attack")
        {
            NextState = idleState
        });

        creatureAnimator.AddAnyState(CreatureAnimator.deathTrigger, new AnimState("Die"));

        return creatureAnimator;
    }
}