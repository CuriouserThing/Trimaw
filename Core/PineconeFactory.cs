using System.Reflection;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using Trimaw.Core.Animation;
using Trimaw.Core.ImaginationSystem;
using Trimaw.Core.Models.Cards;
using Trimaw.Core.Models.Cards.Snacks;
using Trimaw.Core.Models.Enchantments;
using Trimaw.Core.Models.Monsters;
using Trimaw.Core.SnackSystem;

namespace Trimaw.Core;

/// <summary>
///     Main implementation of <see cref="ITrimawCombatManager" /> while Trimaw is in development, and maybe forever.
///     Design matters that originate outside the bounds of vanilla STS2 are centralized here.
/// </summary>
public class PineconeFactory : ITrimawCombatManagerFactory
{
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

        return new PassthroughTrimawCombatManager(
            new MegaRngSnackFactoryPrepManager(SnackFactoryInternal, prepRng),
            new MegaRngFigmentPoolFilterer(FigmentPoolInternal, figmentRng),
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

        new UpgradedRecipe<ChicoTequila>(
            [Enchant<Spiced>(Morsel.Pepper), Enchant<Xxl>(Morsel.Water)],
            [Morsel.Berries, Morsel.Berries, Morsel.Water]),

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

    #region Internals

    private static readonly SpireField<PlayerCombatState, PassthroughTrimawCombatManager> PlayerCombatStateTable =
        new(() => null);

    private ISnackFactory SnackFactoryInternal => field ??= SnackFactory;

    private IReadOnlyList<Figment> FigmentPoolInternal { get; } =
    [
        .. Assembly.GetExecutingAssembly().ExportedTypes
            .Where(TypeIsConcreteFigmentType)
            .Select(GetFigmentModel)
    ];

    private static bool TypeIsConcreteFigmentType(Type type)
    {
        return type.IsAssignableTo(typeof(Figment)) &&
               !type.IsAbstract &&
               !type.ContainsGenericParameters;
    }

    private static Figment GetFigmentModel(Type type)
    {
        var method = typeof(ModelDb).GetMethod(nameof(ModelDb.Monster), BindingFlags.Public | BindingFlags.Static);
        var genericMethod = method!.MakeGenericMethod(type);
        var result = genericMethod.Invoke(null, null) as Figment;
        return result!;
    }

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

    private static IMorselEnchanter Enchant<T>(Morsel morsel) where T : EnchantmentModel
    {
        return new MorselEnchanter<T>(morsel);
    }

    private static IMorselEnchanter Enchant<T>(int amount, Morsel morsel) where T : EnchantmentModel
    {
        return new MorselEnchanter<T>(morsel, amount);
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