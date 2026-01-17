
using MudBlazor;

namespace RC2K.Presentation.Shared;

public class Rc2kColorPallete
{
    public const string primary50 = "#e8eaf7";
    public const string primary100 = "#c5c9ea";
    public const string primary200 = "#a0a6dc";
    public const string primary300 = "#7a83ce";
    public const string primary400 = "#5d67c3";
    public const string primary500 = "#424cb8";
    public const string primary600 = "#3c44ae";
    public const string primary700 = "#333aa2"; // <-- primary
    public const string primary800 = "#2b3096";
    public const string primary900 = "#1d1d82";

    public const string complementary50 = "#fafbe8";
    public const string complementary100 = "#f2f3c5";
    public const string complementary200 = "#e9eca0";
    public const string complementary300 = "#e1e57b";
    public const string complementary400 = "#dade60";
    public const string complementary500 = "#d4d946";
    public const string complementary600 = "#c6c741";
    public const string complementary700 = "#b4b139";
    public const string complementary800 = "#a29b33"; // <-- secondary
    public const string complementary900 = "#847528";

    public const string gray50 = "#FAFAFA";
    public const string gray100 = "#F5F5F5";
    public const string gray200 = "#EEEEEE";
    public const string gray300 = "#E0E0E0";
    public const string gray400 = "#BDBDBD";
    public const string gray500 = "#757575";
    public const string gray600 = "#757575";
    public const string gray700 = "#616161";
    public const string gray800 = "#424242";
    public const string gray900 = "#212121";

    public const string primary = primary700;
    public const string complementary = complementary800;

    public static MudTheme MyCustomTheme = new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = Rc2kColorPallete.primary700,
            Secondary = Rc2kColorPallete.complementary800,
        },
        PaletteDark = new PaletteDark()
        {
            Primary = Rc2kColorPallete.primary200,
            Secondary = Rc2kColorPallete.complementary100
        },
    };
}

