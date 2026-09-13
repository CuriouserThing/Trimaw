namespace Trimaw.Core.SnackSystem;

public static class Printing
{
    private static readonly Dictionary<Morsel, string> EmojiMap = new()
    {
        [Morsel.Bread] = "🥖",
        [Morsel.Meat] = "🍗",
        [Morsel.Shrooms] = "🍄",
        [Morsel.Berries] = "🍇", // Berry emoji display issues
        [Morsel.Pepper] = "🔥", // Pepper emoji display issues
        [Morsel.Water] = "💧"
    };

    public static string Emojify(params IEnumerable<Morsel> morsels)
    {
        return string.Concat(morsels.Select(c => EmojiMap[c]));
    }

    public static string ToEmoji(this Morsel morsel)
    {
        return EmojiMap[morsel];
    }
}