
namespace RC2K.Presentation.Blazor.ViewModels;

public enum Icon
{
    Home,
    Car,
    Stage,
    Contest,
    Mod,
    Snow,
    Sun,
    Fog,
    Cloud,
    ModeLight,
    ModeAuto,
    ModeDark,
    Logout,
    Settings,
    Score,
}

public static class IconMapper
{
    private static readonly Dictionary<Icon, (bool, string, string)> _icon2classMap;

    static IconMapper()
    {
        _icon2classMap = new Dictionary<Icon, (bool, string, string)>
        {
            { Icon.Home, (true, MudBlazor.Icons.Material.Outlined.Home, MudBlazor.Icons.Material.Filled.Home) },
            { Icon.Car, (true, MudBlazor.Icons.Material.Outlined.DirectionsCarFilled, MudBlazor.Icons.Material.Filled.DirectionsCarFilled) },
            { Icon.Stage, (true, MudBlazor.Icons.Material.Outlined.Map, MudBlazor.Icons.Material.Filled.Map) },
            { Icon.Contest, (true, MudBlazor.Icons.Material.Outlined.Flag, MudBlazor.Icons.Material.Filled.Flag) },
            { Icon.Mod, (false, "fa-regular fa-pen-to-square", "fa-solid fa-pen-to-square") },
            { Icon.Snow, (false, "fa-regular fa-snowflake", "fa-solid fa-snowflake") },
            { Icon.Sun, (true, MudBlazor.Icons.Material.Outlined.WbSunny, MudBlazor.Icons.Material.Filled.WbSunny) },
            { Icon.Fog, (false, "fa-solid fa-smog", "fa-solid fa-smog") },
            { Icon.Cloud, (true, MudBlazor.Icons.Material.Outlined.WbCloudy, MudBlazor.Icons.Material.Filled.WbCloudy) },
            { Icon.ModeLight, (true, MudBlazor.Icons.Material.Outlined.LightMode, MudBlazor.Icons.Material.Filled.LightMode) },
            { Icon.ModeAuto, (true, MudBlazor.Icons.Material.Outlined.AutoMode, MudBlazor.Icons.Material.Filled.AutoMode) },
            { Icon.ModeDark, (true, MudBlazor.Icons.Material.Outlined.DarkMode, MudBlazor.Icons.Material.Filled.DarkMode) },
            { Icon.Logout, (true, MudBlazor.Icons.Material.Outlined.Logout, MudBlazor.Icons.Material.Filled.Logout) },
            { Icon.Settings, (true, MudBlazor.Icons.Material.Outlined.Settings, MudBlazor.Icons.Material.Filled.Settings) },
            { Icon.Score, (true, MudBlazor.Icons.Material.Outlined.TrendingUp, MudBlazor.Icons.Material.Filled.TrendingUp) },
        };
    }

    public static bool IsMudBlazor(Icon icon) => _icon2classMap[icon].Item1;
    public static string Thin(Icon icon) => _icon2classMap[icon].Item2;
    public static string Thick(Icon icon) => _icon2classMap[icon].Item3;
}