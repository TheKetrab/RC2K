using RC2K.DomainModel;

namespace RC2K.Presentation.Blazor.Views.Components;


public class RankingListItem
{
    public RankingEntry Data { get; }

    public RankingListItem(RankingEntry data)
    {
        Data = data;
    }

    public string DisplayName => Data.Driver!.Known ? Data.Driver.User!.Name! : Data.Driver.Name!;

    public int CarPoints =>
        Data.CarA8Points +
        Data.CarA7Points +
        Data.CarA6Points +
        Data.CarA5Points +
        Data.CarBonusPoints;

    public int TotalPoints => 
        Data.GeneralPoints +
        CarPoints +
        Data.BonusPoints;

}


