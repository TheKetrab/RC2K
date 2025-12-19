
namespace RC2K.Presentation.Blazor.ViewModels.Layout;

public class HeaderViewModel : BaseViewModel
{
    public List<MenuItem> MenuItems = 
    [
        new MenuItem() { Item = "Home", ItemLink = "/home", Icon = Icon.Home },
        new MenuItem() { Item = "Cars", ItemLink = "/cars", Icon = Icon.Car, Subitems = [
            ("Class A5","cars/a5"),
            ("Class A6","cars/a6"),
            ("Class A7","cars/a7"),
            ("Class A8","cars/a8"),
            ("Bonus","cars/bonus"),
        ] },
        new MenuItem() { Item = "Stages", ItemLink = "/stages", Icon = Icon.Stage, Subitems = [
            ("Vauxhall Rally of Wales","stages/vauxhall"),
            ("Pirelli International Rally","stages/pirelli"),
            ("Scottish Rally RSAC","stages/scottish"),
            ("SEAT Jim Clark Memorial Rally","stages/seat"),
            ("Stena Line Ulster Rally","stages/stena"),
            ("SONY Manx International Rally","stages/sony"),
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
