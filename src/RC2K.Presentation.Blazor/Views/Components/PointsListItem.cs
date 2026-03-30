using RC2K.DomainModel;

namespace RC2K.Presentation.Blazor.Views.Components;

public class PointsListItem
{
    private PointsList list;
    public PointsListItem(PointsList list,
        Driver driver,
        int place,
        int totalPoints,
        int generalPoints,
        int carA8Points,
        int carA7Points,
        int carA6Points,
        int carA5Points,
        int carBonusPoints)
    {
        Driver = driver;
        Place = place;
        TotalPoints = totalPoints;
        GeneralPoints = generalPoints;
        CarA8Points = carA8Points;
        CarA7Points = carA7Points;
        CarA6Points = carA6Points;
        CarA5Points = carA5Points;
        CarBonusPoints = carBonusPoints;
        this.list = list;
    }

    public int Place { get; set; }

    public int TotalPoints { get; set; }

    public Driver Driver { get; set; }

    public int? Gap => list.Best is not null
        ? this.TotalPoints - list.Best
        : 0;

    public string DisplayName => Driver!.Known ? Driver.User!.Name! : Driver.Name!;

    public int GeneralPoints { get; set; }
    public int CarA8Points { get; set; }
    public int CarA7Points { get; set; }
    public int CarA6Points { get; set; }
    public int CarA5Points { get; set; }
    public int CarBonusPoints { get; set; }

}


