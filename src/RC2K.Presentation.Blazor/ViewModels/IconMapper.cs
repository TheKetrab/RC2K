
using RC2K.DomainModel;

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
    YouTube,
    Twitch,
    Replay,
    Image,
    Unknown
}

public static class IconMapper
{
    private static readonly Dictionary<Icon, (bool, string, string)> _icon2classMap;

    private const string twitch = @"
    <svg style=""width:16px;height:16px"" viewBox=""0 0 16 16"">""
        <path d=""M3.857 0 1 2.857v10.286h3.429V16l2.857-2.857H9.57L14.714 8V0zm9.714 7.429-2.285 2.285H9l-2 2v-2H4.429V1.143h9.142z""/>
        <path d=""M11.857 3.143h-1.143V6.57h1.143zm-3.143 0H7.571V6.57h1.143z""/>
    </svg>
    ";

    private const string camera_slim = @"
    <svg style=""width:16px;height:16px"" viewBox=""0 0 16 16"">""
        <path d=""M6 3a3 3 0 1 1-6 0 3 3 0 0 1 6 0M1 3a2 2 0 1 0 4 0 2 2 0 0 0-4 0""/>
        <path d=""M9 6h.5a2 2 0 0 1 1.983 1.738l3.11-1.382A1 1 0 0 1 16 7.269v7.462a1 1 0 0 1-1.406.913l-3.111-1.382A2 2 0 0 1 9.5 16H2a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2zm6 8.73V7.27l-3.5 1.555v4.35zM1 8v6a1 1 0 0 0 1 1h7.5a1 1 0 0 0 1-1V8a1 1 0 0 0-1-1H2a1 1 0 0 0-1 1""/>
        <path d=""M9 6a3 3 0 1 0 0-6 3 3 0 0 0 0 6M7 3a2 2 0 1 1 4 0 2 2 0 0 1-4 0""/>
    </svg>
    ";

    private const string camera_fill = @"
    <svg style=""width:16px;height:16px"" viewBox=""0 0 16 16"">""
        <path d=""M6 3a3 3 0 1 1-6 0 3 3 0 0 1 6 0""/>
        <path d=""M9 6a3 3 0 1 1 0-6 3 3 0 0 1 0 6""/>
        <path d=""M9 6h.5a2 2 0 0 1 1.983 1.738l3.11-1.382A1 1 0 0 1 16 7.269v7.462a1 1 0 0 1-1.406.913l-3.111-1.382A2 2 0 0 1 9.5 16H2a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2z""/>
    </svg>
    ";

    private const string image_slim = @"
    <svg style=""width:16px;height:16px"" viewBox=""0 0 16 16"">""
        <path d=""M6.002 5.5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0""/>
        <path d=""M2.002 1a2 2 0 0 0-2 2v10a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V3a2 2 0 0 0-2-2zm12 1a1 1 0 0 1 1 1v6.5l-3.777-1.947a.5.5 0 0 0-.577.093l-3.71 3.71-2.66-1.772a.5.5 0 0 0-.63.062L1.002 12V3a1 1 0 0 1 1-1z""/>
    </svg>
    ";

    private const string image_fill = @"
    <svg style=""width:16px;height:16px"" viewBox=""0 0 16 16"">""
        <path d=""M.002 3a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v10a2 2 0 0 1-2 2h-12a2 2 0 0 1-2-2zm1 9v1a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1V9.5l-3.777-1.947a.5.5 0 0 0-.577.093l-3.71 3.71-2.66-1.772a.5.5 0 0 0-.63.062zm5-6.5a1.5 1.5 0 1 0-3 0 1.5 1.5 0 0 0 3 0""/>
    </svg>
    ";

    private const string square_slim = @"
    <svg style=""width:16px;height:16px"" viewBox=""0 0 16 16"">""
        <path d=""M14 1a1 1 0 0 1 1 1v12a1 1 0 0 1-1 1H2a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1zM2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2z""/>
    </svg>
    ";

    private const string square_fill = @"
    <svg style=""width:16px;height:16px"" viewBox=""0 0 16 16"">""
        <path d=""M0 2a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2z""/>
    </svg>
    ";


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
            { Icon.YouTube, (true, MudBlazor.Icons.Custom.Brands.YouTube, MudBlazor.Icons.Custom.Brands.YouTube) },
            { Icon.Twitch, (true, twitch, twitch) },
            { Icon.Replay, (true, camera_slim, camera_fill) },
            { Icon.Image, (true, image_slim, image_fill) },
            { Icon.Unknown, (true, square_slim, square_fill) },
        };
    }

    public static bool IsMudBlazor(Icon icon) => _icon2classMap[icon].Item1;
    public static string Thin(Icon icon) => _icon2classMap[icon].Item2;
    public static string Thick(Icon icon) => _icon2classMap[icon].Item3;

    public static string ProofTypeToIcon(ProofType proofType) => proofType switch
    {
        ProofType.Youtube => Thin(Icon.YouTube),
        ProofType.Twitch => Thin(Icon.Twitch),
        ProofType.Replay => Thin(Icon.Replay),
        ProofType.Image => Thin(Icon.Image),
        ProofType.Unknown => Thin(Icon.Unknown),
    };
}