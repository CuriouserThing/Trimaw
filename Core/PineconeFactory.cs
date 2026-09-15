using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Cards;
using Trimaw.Core.Models.Cards.Snacks;
using Trimaw.Core.Models.Enchantments;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.Models.Monsters.Figments;
using Trimaw.Core.SnackSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core;

/// <summary>
///     Main implementation of <see cref="ITrimawCombatManager" /> while Trimaw is in development, and maybe forever.
///     Design matters that originate outside the bounds of vanilla STS2 are centralized here.
/// </summary>
public class PineconeFactory : ITrimawCombatManagerFactory
{
    #region Super Special Enchantment!

    public static IReadOnlyList<IEnchanter> SuperSpecialEnchantmentPool =>
    [
        // Is ANY card (try to keep at least a couple here)
        Enchant<Glam>(),
        Enchant<ReallyHot>(4),
        
        // Costs 0-1
        Enchant<Hyperfixated>(),

        // Costs 1+ and doesn't exhaust
        Enchant<Polished>(),

        // Costs 2+ and either deals damage or gains block
        Enchant<Doodled>(),

        // Exhausts
        Enchant<Xxl>(2),

        // Is an Attack
        Enchant<Inky>(c => c.Type == CardType.Attack), // vanilla doesn't do this
        Enchant<Momentum>(3),
        Enchant<Corrupted>(),

        // Is a targeted Attack
        Enchant<Focused>(),

        // Is an Attack that deals N+ damage
        Enchant<ReallyHeavy>(),

        // Gains block
        Enchant<Nimble>(3)
    ];

    #endregion

    private PassthroughTrimawCombatManager Create(Player player)
    {
        var ceobeAnimator = new StandardCeobeAnimator
        {
            BaselineStaffAttackTimescale = 2.0f,
            BaselineAxeAttackTimescale = 1.5f,
            BaselineSpearAttackTimescale = 1.5f,
            BaselineKnifeAttackTimescale = 1.0f
        };

        // Uses CombatCardGeneration RNG because prepped morsels are effectively fractional cards 
        var prepRng = player.RunState.Rng.CombatCardGeneration;

        // Uses MonsterAi RNG because the choice of figment is also a choice of intended monster move
        var figmentRng = player.RunState.Rng.MonsterAi;

        // Uses CombatCardGeneration RNG because [super special] enchantments are generated card mods
        var superSpecialRng = player.RunState.Rng.CombatCardGeneration;

        return new PassthroughTrimawCombatManager(
            new MegaRngSnackFactoryPrepManager(SnackFactoryInternal, prepRng),
            new MegaRngFigmentPoolFilterer(FigmentPoolInternal, StandardPrepass, GachaPrepass, figmentRng),
            new MegaRngEnchantmentPoolSuperSpecializer(SuperSpecialEnchantmentPoolInternal, superSpecialRng),
            ceobeAnimator);
    }

    #region Snacking

    public static ISnackFactory SnackFactory => new SnackFactory(SnackMenu, new FallbackSnackFactory<JunkFood>())
    {
        UseFifoMorsels = true
    };

    public static SnackMenu SnackMenu => new(RecipeBook);

    public static IReadOnlyList<IRecipe> RecipeBook =>
    [
        new UpgradedRecipe<BarrensTequila>(
            [Enchant<Spiced>(Morsel.Pepper), Enchant<Xxl>(Morsel.Water)],
            [Morsel.Berries, Morsel.Berries, Morsel.Water]),

        new Recipe<BloodCurd>(
            [Morsel.Meat, Morsel.Meat],
            Morsel.Meat),

        new Recipe<BobsBeer>(
            [Enchant<Xxl>(Morsel.Water), Enchant<Spiced>(Morsel.Pepper)],
            [Morsel.Bread, Morsel.Water],
            Morsel.Bread),

        new Recipe<BoxOfTruffles>(
            [Enchant<Glazed>(Morsel.Berries)],
            [Morsel.Shrooms, Morsel.Berries],
            Morsel.Shrooms),

        new UpgradedRecipe<BoneAppleTea>(
            [Morsel.Meat, Morsel.Berries, Morsel.Water]),

        new Recipe<Byrdomelet>(
            [Morsel.Meat, Morsel.Pepper],
            Morsel.Meat, Morsel.Pepper),

        new Recipe<CaramelCandies>(
            [Enchant<Spiced>(Morsel.Pepper), Enchant<Glazed>(Morsel.Berries)],
            [Morsel.Berries, Morsel.Berries]),

        new UpgradedRecipe<Xxl, Cargirl>(
            [Morsel.Water, Morsel.Water, Morsel.Water, Morsel.Water]),

        new UpgradedRecipe<CrabPorridge>(
            [Enchant<ReallyHot>(3, Morsel.Pepper)],
            [Morsel.Meat, Morsel.Bread, Morsel.Water]),

        new UpgradedRecipe<FairyRing>(
            [Morsel.Shrooms, Morsel.Shrooms, Morsel.Shrooms]),

        new UpgradedRecipe<GummyDumpling>(
            [
                Enchant<Xxl>(Morsel.Bread),
                Enchant<ReallyHot>(3, Morsel.Pepper),
                Enchant<Skewered>(Morsel.Meat)
            ],
            [Morsel.Bread, Morsel.Bread, Morsel.Shrooms]),

        new Recipe<HoneyBiscuit>(
            [
                Enchant<Xxl>(Morsel.Bread),
                Enchant<Spiced>(Morsel.Pepper),
                Enchant<Slimy>(Morsel.Shrooms)
            ],
            [Morsel.Bread, Morsel.Berries],
            Morsel.Berries),

        new Recipe<HotSauce>(
            [Enchant<ReallyHot>(3, Morsel.Pepper)],
            [Morsel.Pepper, Morsel.Pepper]),

        new Recipe<InsanityPepper>(
            [Enchant<Skewered>(Morsel.Pepper)],
            [Morsel.Pepper, Morsel.Shrooms]),

        new Recipe<Pocketed, Kebab>(
            [Morsel.Meat, Morsel.Shrooms, Morsel.Bread],
            Morsel.Meat),
        new Recipe<Skewered, Kebab>(
            [Morsel.Meat, Morsel.Shrooms, Morsel.Meat],
            Morsel.Meat),

        new UpgradedRecipe<MdPepper>(
            [Enchant<Xxl>(Morsel.Water)],
            [Morsel.Berries, Morsel.Pepper, Morsel.Water]),

        new UpgradedRecipe<Xxl, No1ShioRamen>(
            [Morsel.Bread, Morsel.Water, Morsel.Water, Morsel.Water]),

        new Recipe<ShavedIce>(
            [Enchant<Glazed>(Morsel.Berries)],
            [Morsel.Water, Morsel.Water],
            Morsel.Water),

        new Recipe<SlugSpread>(
            [
                Enchant<Skewered>(Morsel.Pepper),
                Enchant<Slimy>(Morsel.Shrooms)
            ],
            [Morsel.Meat, Morsel.Berries],
            Morsel.Berries),

        new Recipe<TacoDossoleado>(
            [Enchant<Xxl>(Morsel.Bread), Enchant<ReallyHot>(3, Morsel.Pepper)],
            [Morsel.Bread, Morsel.Meat],
            Morsel.Meat),

        new Recipe<Xxl, UrsusBigBread>(
            [Morsel.Bread, Morsel.Bread, Morsel.Bread],
            Morsel.Bread),

        new Recipe<VillagePot>(
            [Morsel.Meat, Morsel.Shrooms],
            Morsel.Shrooms)
    ];

    #endregion

    #region Imagination

    public static FigmentFilter StandardPrepass { get; } = FigmentFilter.All
        .Multiplying(SoftTag.CommonRarity, 1.2M)
        .Multiplying(SoftTag.UncommonRarity, 1.0M)
        .Forbidding(SoftTag.ExcludedFromAllRolls);

    public static FigmentFilter GachaPrepass { get; } = FigmentFilter.All
        .Forbidding(SoftTag.ExcludedFromGacha);

    public static IReadOnlyList<TaggedFigment> FigmentPool =>
    [
        // Generic
        Tag<BubbleFigment>(SoftTag.CommonRarity),
        Tag<SesaFigment>(SoftTag.UncommonRarity),
        Tag<ShamareFigment>(SoftTag.UncommonRarity),
        Tag<VulcanFigment>(SoftTag.UncommonRarity),

        // Sluggies
        Tag<BasicSlugFigment>(SoftTag.CommonRarity),
        Tag<FluorescentSlugFigment>(SoftTag.CommonRarity),
        Tag<BerrySlugFigment>(SoftTag.CommonRarity),

        // ReserveOp
        Tag<ReserveVanguardFigment>(SoftTag.CommonRarity),
        Tag<ReserveGuardFigment>(SoftTag.CommonRarity),
        Tag<ReserveDefenderFigment>(SoftTag.CommonRarity),
        Tag<ReserveSniperFigment>(SoftTag.CommonRarity),
        Tag<ReserveCasterFigment>(SoftTag.CommonRarity),
        Tag<ReserveMedicFigment>(SoftTag.CommonRarity),
        Tag<ReserveSupporterFigment>(SoftTag.CommonRarity),
        Tag<ReserveSpecialistFigment>(SoftTag.CommonRarity),

        // Tiacauh
        Tag<TiacauhImpalerFigment>(SoftTag.CommonRarity),
        Tag<TiacauhRitualistFigment>(SoftTag.CommonRarity),
        Tag<TiacauhFanaticFigment>(SoftTag.CommonRarity),
        Tag<TiacauhShredderFigment>(SoftTag.CommonRarity),
        Tag<FlintFigment>(SoftTag.UncommonRarity),
        Tag<TomimiFigment>(SoftTag.UncommonRarity),
        Tag<GavialFigment>(SoftTag.UncommonRarity),
        Tag<EunectesFigment>(SoftTag.UncommonRarity),
        Tag<BigUglyThingFigment>(SoftTag.ExcludedFromAllRolls),

        // DuckLordAssociate
        Tag<DuckLordFigment>(SoftTag.UncommonRarity, SoftTag.ExcludedFromGacha),
        Tag<CryingThiefFigment>(SoftTag.UncommonRarity),
        Tag<FattyFigment>(SoftTag.UncommonRarity),

        // DuckLordAssociate & UrsusRace
        Tag<GopnikFigment>(SoftTag.UncommonRarity),

        // UrsusRace
        Tag<BeehunterFigment>(SoftTag.CommonRarity),
        Tag<GummyFigment>(SoftTag.CommonRarity),
        Tag<IstinaFigment>(SoftTag.UncommonRarity),
        Tag<RosaFigment>(SoftTag.UncommonRarity),
        Tag<ZimaFigment>(SoftTag.UncommonRarity)
    ];

    #endregion

    #region Internals

    private static readonly SpireField<PlayerCombatState, PassthroughTrimawCombatManager> PlayerCombatStateTable =
        new(() => null);

    private ISnackFactory SnackFactoryInternal => field ??= SnackFactory;
    private IReadOnlyList<TaggedFigment> FigmentPoolInternal => field ??= FigmentPool;
    private IReadOnlyList<IEnchanter> SuperSpecialEnchantmentPoolInternal => field ??= SuperSpecialEnchantmentPool;

    public ITrimawCombatManager Register(Player player, PlayerCombatState combatState)
    {
        var value = Create(player);
        PlayerCombatStateTable.Set(combatState, value);
        return value;
    }

    public ITrimawCombatManager GetOrCreate(Player player)
    {
        if (player.PlayerCombatState is { } combatState)
            return PlayerCombatStateTable.Get(combatState) ?? Register(player, combatState);
        return Create(player);
    }

    private static IEnchanter Enchant<T>(Predicate<CardModel>? canPotentiallyEnchantPredicate = null)
        where T : EnchantmentModel
    {
        return new Enchanter<T>
        {
            CanPotentiallyEnchantPredicate = canPotentiallyEnchantPredicate
        };
    }

    private static IEnchanter Enchant<T>(int amount, Predicate<CardModel>? canPotentiallyEnchantPredicate = null)
        where T : EnchantmentModel
    {
        return new Enchanter<T>(amount)
        {
            CanPotentiallyEnchantPredicate = canPotentiallyEnchantPredicate
        };
    }

    private static IMorselEnchanter Enchant<T>(Morsel morsel) where T : EnchantmentModel
    {
        return new MorselEnchanter<T>(morsel);
    }

    private static IMorselEnchanter Enchant<T>(int amount, Morsel morsel) where T : EnchantmentModel
    {
        return new MorselEnchanter<T>(morsel, amount);
    }

    private static TaggedFigment<T> Tag<T>(params SoftTag[] softTags) where T : Figment, new()
    {
        var canonFigment = ModelDb.Monster<T>();
        var tags = new HashSet<FigmentFilter.Tag>();
        foreach (var tag in canonFigment.HardTags) tags.Add(tag);
        foreach (var tag in softTags.Distinct()) tags.Add(tag);
        return new TaggedFigment<T>(tags);
    }

    private class FallbackSnackFactory<T> : ISnackFactory where T : SnackCard
    {
        public SnackResult PullFromStock(ReadOnlySpan<Morsel> stock)
        {
            var snackBase = new Morsel[stock.Length];
            stock.CopyTo(snackBase);
            var snack = EncodedSnack.FromMorsels<T>(snackBase);
            return new SnackResult(snack, (1 << stock.Length) - 1);
        }
    }

    #endregion
}