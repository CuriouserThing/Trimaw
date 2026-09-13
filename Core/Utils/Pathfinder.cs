using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace Trimaw.Core.Utils;

public static class Pathfinder
{
    public static string ResPath => $"res://{MainFile.ModId}";

    private static void DashToUnderscore(Span<string> name)
    {
        for (var i = 0; i < name.Length; i += 1) name[i] = name[i].Replace('-', '_');
    }

    private static void UnderscoreToDash(Span<string> name)
    {
        for (var i = 0; i < name.Length; i += 1) name[i] = name[i].Replace('_', '-');
    }

    private static string ModelFile(AbstractModel model)
    {
        return model.Id.Entry.RemovePrefix().ToLowerInvariant();
    }

    private static string? Found(this string path)
    {
        if (ResourceLoader.Exists(path)) return path;

        MainFile.Logger.Debug($"Resource path not found: {path}");
        return null;
    }

    public static string Scene(params string[] name)
    {
        DashToUnderscore(name);
        return Path.Join(ResPath, "scenes", $"{Path.Join(name)}.tscn");
    }

    public static string AkSkeletonDataRes(params string[] name)
    {
        DashToUnderscore(name);
        return Path.Join(ResPath, "ak_spine", $"{Path.Join(name)}.tres");
    }

    public static string Image(params string[] name)
    {
        return Path.Join(ResPath, "images", $"{Path.Join(name)}.png");
    }

    public static string UiImage(params string[] name)
    {
        return Image("ui", Path.Join(name));
    }

    public static string CardPortrait(AbstractModel model)
    {
        return Image("card_portraits", $"{ModelFile(model)}").Found() ??
               Image("card_portraits", "placeholder");
    }

    public static string BetaCardPortrait(AbstractModel model)
    {
        return Image("card_portraits", "beta", $"{ModelFile(model)}").Found() ??
               CardPortrait(model);
    }

    public static string Enchantment64(AbstractModel model)
    {
        return Image("enchantments", $"{ModelFile(model)}").Found() ??
               NotoEmoji64("sparkles");
    }

    public static string Potion80(AbstractModel model)
    {
        return Image("potions", $"{ModelFile(model)}").Found() ??
               NotoEmoji80("bubble_tea");
    }

    public static string PotionOutline80(AbstractModel model)
    {
        return Image("potions", $"{ModelFile(model)}_outline").Found() ??
               Image("potions", "placeholder_outline");
    }

    public static string Potion256(AbstractModel model)
    {
        return Image("potions", "big", $"{ModelFile(model)}").Found() ??
               NotoEmoji256("bubble_tea");
    }

    public static string Power64(AbstractModel model)
    {
        return Image("powers", $"{ModelFile(model)}").Found() ??
               NotoEmoji64("battery");
    }

    public static string Power256(AbstractModel model)
    {
        return Image("powers", "big", $"{ModelFile(model)}").Found() ??
               NotoEmoji256("battery");
    }

    public static string VanillaPower64<T>() where T : PowerModel
    {
        return ModelDb.Power<T>().PackedIconPath.Found() ??
               Image("powers", "power");
    }

    public static string Relic85(AbstractModel model)
    {
        return Image("relics", $"{ModelFile(model)}").Found() ??
               NotoEmoji85("package");
    }

    public static string RelicOutline85(AbstractModel model)
    {
        return Image("relics", $"{ModelFile(model)}_outline").Found() ??
               Image("relics", "placeholder_outline");
    }

    public static string Relic256(AbstractModel model)
    {
        return Image("relics", "big", $"{ModelFile(model)}").Found() ??
               NotoEmoji256("package");
    }

    public static string NotoEmoji64(params string[] name)
    {
        DashToUnderscore(name);
        return Image("noto_emoji_64px", $"{Path.Join(name)}");
    }

    public static string NotoEmoji80(params string[] name)
    {
        DashToUnderscore(name);
        return Image("noto_emoji_80px", $"{Path.Join(name)}");
    }

    public static string NotoEmoji85(params string[] name)
    {
        DashToUnderscore(name);
        return Image("noto_emoji_85px", $"{Path.Join(name)}");
    }

    public static string NotoEmoji256(params string[] name)
    {
        DashToUnderscore(name);
        return Image("noto_emoji_256px", $"{Path.Join(name)}");
    }

    public static string FluentUiEmoji256Color(params string[] name)
    {
        DashToUnderscore(name);
        return Image("fluent_ui_emoji_256px", $"{Path.Join(name)}_color");
    }

    public static string GameIconsDotnet64(params string[] name)
    {
        UnderscoreToDash(name);
        return Image("game_icons_dotnet_64px", $"{Path.Join(name)}");
    }

    public static string GameIconsDotnet256(params string[] name)
    {
        UnderscoreToDash(name);
        return Image("game_icons_dotnet_256px", $"{Path.Join(name)}");
    }
}