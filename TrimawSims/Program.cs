using Trimaw.Core;
using Trimaw.Core.SnackSystem;
using TrimawSims.Snacking;

var prepSim = new PrepSim(
    MegaRngSnackFactoryPrepManager.FromRandomSeed(PineconeFactory.SnackFactory),
    new Random(), // only for sim-specific rolls (not anything within STS2 so don't even touch STS2 RNGs here)
    new Dictionary<Selection, int>
    {
        [Selection.PrepRandom] = 100, // "Prep a random Morsel"; starting relic etc.
        [Selection.PrepWeighted] = 0, // "Prep ___" (see morsel weights below)
        [Selection.PrepLast] = 25, // "Prep the last Morsel you prepped"
        [Selection.ClearAllMorsels] = 10 // command representing the end of combat
    },
    new Dictionary<Morsel, int>
    {
        [Morsel.Shrooms] = 130,
        [Morsel.Bread] = 120,
        [Morsel.Meat] = 100,
        [Morsel.Berries] = 100,
        [Morsel.Pepper] = 80,
        [Morsel.Water] = 70
    });

const int targetSnackCount = 1_000_000;
var snacks = new List<EncodedSnack>(targetSnackCount);
var leftovers = new int[SnackConstants.MorselKinds];
while (snacks.Count < targetSnackCount)
    if (prepSim.Next() is { } snack)
    {
        snacks.Add(snack);
        foreach (var leftover in prepSim.GetMorsels()) leftovers[(int)leftover] += 1;
    }

Console.WriteLine();
Console.WriteLine("====================================================================");
Console.WriteLine("Simulation results:");
Console.WriteLine();
Console.WriteLine("--------------------------------------------------------------------");
Console.WriteLine();

foreach (var group in snacks
             .GroupBy(s => s.Recipe)
             .OrderByDescending(g => g.Count()))
{
    var design = group.Key;
    var designCount = group.Count();
    var pct = (float)designCount / snacks.Count;
    Console.WriteLine($"{pct * 100,5:#0.00}%: {design}");

    foreach (var subgroup in group
                 .GroupBy(s => s.Code)
                 .OrderByDescending(g => g.Count()))
    {
        var permCount = subgroup.Count();
        if (permCount == designCount) break; // don't bother with lone permutations

        var snack = subgroup.First(); // doesn't matter which
        var localPct = (float)permCount / designCount;
        var globalPct = (float)permCount / snacks.Count;
        Console.WriteLine($"\t{globalPct * 100,5:#0.00}% ({localPct * 100,5:#0.00}%): {snack}");
    }
}

Console.WriteLine();
Console.WriteLine("--------------------------------------------------------------------");
Console.WriteLine();

foreach (var (upgraded, outerCount) in snacks
             .CountBy(s => s.IsUpgraded)
             .OrderByDescending(kvp => kvp.Value))
{
    var pct = (float)outerCount / snacks.Count;
    var cat = upgraded ? "Upgraded, using..." : "Un-upgraded";
    Console.WriteLine($"{pct * 100,5:#0.00}%: {cat}");
    if (!upgraded) continue;

    foreach (var (upgrader, innerCount) in snacks
                 .Where(s => s.IsUpgraded)
                 .CountBy(s => s.MaybeUpgrader?.ToEmoji() ?? "Nothing (pre-upgraded)")
                 .OrderByDescending(kvp => kvp.Value))
    {
        var localPct = (float)innerCount / outerCount;
        var globalPct = (float)innerCount / snacks.Count;
        Console.WriteLine($"\t{globalPct * 100,5:#0.00}% ({localPct * 100,5:#0.00}%): {upgrader}");
    }
}

Console.WriteLine();
Console.WriteLine("--------------------------------------------------------------------");
Console.WriteLine();

foreach (var (enchanted, outerCount) in snacks
             .CountBy(s => s.IsEnchanted)
             .OrderByDescending(kvp => kvp.Value))
{
    var pct = (float)outerCount / snacks.Count;
    var cat = enchanted ? "Enchanted, with..." : "Un-enchanted";
    Console.WriteLine($"{pct * 100,5:#0.00}%: {cat}");
    if (!enchanted) continue;

    foreach (var (enchanter, innerCount) in snacks
                 .Where(s => s.IsEnchanted)
                 .CountBy(s => s.MaybeEnchanter?.ToString() ?? string.Empty)
                 .OrderByDescending(kvp => kvp.Value))
    {
        var localPct = (float)innerCount / outerCount;
        var globalPct = (float)innerCount / snacks.Count;
        Console.WriteLine($"\t{globalPct * 100,5:#0.00}% ({localPct * 100,5:#0.00}%): {enchanter}");
    }
}

Console.WriteLine();
Console.WriteLine("--------------------------------------------------------------------");
Console.WriteLine();

foreach (var (enchanted, outerCount) in snacks
             .CountBy(s => s.IsEnchanted)
             .OrderByDescending(kvp => kvp.Value))
{
    var pct = (float)outerCount / snacks.Count;
    var cat = enchanted ? "Enchanted, using..." : "Un-enchanted";
    Console.WriteLine($"{pct * 100,5:#0.00}%: {cat}");
    if (!enchanted) continue;

    foreach (var (enchanter, innerCount) in snacks
                 .Where(s => s.IsEnchanted)
                 .CountBy(s => s.MaybeMorselEnchanter?.Morsel.ToEmoji() ?? "Nothing (pre-enchanted)")
                 .OrderByDescending(kvp => kvp.Value))
    {
        var localPct = (float)innerCount / outerCount;
        var globalPct = (float)innerCount / snacks.Count;
        Console.WriteLine($"\t{globalPct * 100,5:#0.00}% ({localPct * 100,5:#0.00}%): {enchanter}");
    }
}

Console.WriteLine();
Console.WriteLine("--------------------------------------------------------------------");
Console.WriteLine();

var totalMorsels = 0;
foreach (var (morselCount, snackCount) in snacks
             .CountBy(s => s.MorselCount)
             .OrderBy(kvp => kvp.Key))
{
    totalMorsels += morselCount * snackCount;
    var pct = (float)snackCount / snacks.Count;
    Console.WriteLine($"{morselCount}-morsel snacks: {pct * 100,5:#0.00}%");
}

var meanSnackMorselCount = (float)totalMorsels / snacks.Count;

Console.WriteLine();
Console.WriteLine($"Average snack morsel count: {meanSnackMorselCount,5:#0.00}");

Console.WriteLine();
Console.WriteLine("--------------------------------------------------------------------");
Console.WriteLine("Occasions left-over after snack made:");
Console.WriteLine();

for (var i = 0; i < leftovers.Length; i += 1)
{
    var morsel = (Morsel)i;
    var count = leftovers[i];
    var pct = (float)count / snacks.Count;
    Console.WriteLine($"{morsel.ToEmoji()}: {pct * 100,5:#0.00}%");
}

Console.WriteLine();
Console.WriteLine("====================================================================");