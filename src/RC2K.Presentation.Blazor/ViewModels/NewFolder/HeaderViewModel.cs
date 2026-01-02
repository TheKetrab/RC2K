
using RC2K.DomainModel;
using RC2K.Extensions;

namespace RC2K.Presentation.Blazor.ViewModels.Layout;

public class HeaderViewModel : BaseViewModel
{
    public List<MenuItem> MenuItems = 
    [
        new MenuItem() { Item = "Home", ItemLink = "/home", Icon = Icon.Home },
        new MenuItem() { Item = "Ranking", ItemLink = "/ranking", Icon = Icon.Ranking },
        //new MenuItem() { Item = "Cars", ItemLink = "/cars", Icon = Icon.Car, Subitems = [
        //    ("Class A5","cars/a5"),
        //    ("Class A6","cars/a6"),
        //    ("Class A7","cars/a7"),
        //    ("Class A8","cars/a8"),
        //    ("Bonus","cars/bonus"),
        //] },
        new MenuItem() { Item = "Admin", ItemLink = "/admin", Icon = Icon.Settings, Subitems = [
            ("Verify info","admin/verification"),
        ] },
        new MenuItem() { Item = "Stages", ItemLink = "/stages", Icon = Icon.Stage, Subitems = [
            GetRallySubitem(RallyCode.Vauxhall),
            GetRallySubitem(RallyCode.Pirelli),
            GetRallySubitem(RallyCode.Scottish),
            GetRallySubitem(RallyCode.Seat),
            GetRallySubitem(RallyCode.Stena),
            GetRallySubitem(RallyCode.Sony),
        ] },
        //new MenuItem() { Item = "Contests", ItemLink = "/contests", Icon = Icon.Contest, Subitems = [
        //    ("MFMI23","contests/mfmi23"),
        //    ("The White Heat","contests/twh"),
        //    ("MFMI22","contests/mfmi22"),
        //] },
        //new MenuItem() { Item = "Mods", ItemLink = "/mods", Icon = Icon.Mod, Subitems = [
        //    ("Skins","mods/skins"),
        //    ("Maps","mods/maps"),
        //] },
    ];

    private static (string, string) GetRallySubitem(RallyCode code) =>
        (LevelHelper.RallyCodeToRallyName(code), $"stages/{LevelHelper.RallyCodeToRallyShortName(code)}");

    private bool _navIsSticked;
    public bool NavIsSticked
    {
        get => _navIsSticked;
        set
        {
            if (_navIsSticked != value)
            {
                SetProperty(ref _navIsSticked, value, nameof(NavIsSticked));
            }
        }
    }


}
