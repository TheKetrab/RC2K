using RC2K.DomainModel;

namespace RC2K.Presentation.Blazor.Views.Components;


public class RankingListItem
{
    private RankingList list;
    public RankingListItem(RankingList list, RankingEntry data)
    {
        Data = data;
        this.list = list;
    }

    public RankingEntry Data { get; set; }

    public string DisplayName => Data.Driver.Known ? Data.Driver.User!.Name : Data.Driver.Name;

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


