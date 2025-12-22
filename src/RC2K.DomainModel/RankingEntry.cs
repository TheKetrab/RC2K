namespace RC2K.DomainModel;

public class RankingEntry
{
    public required int Place { get; init; }
    public required Guid DriverId { get; init; }

    public required int GeneralPoints { get; init; }
    public required int GeneralTop30Count { get; init; }
    public required int GeneralTop10Count { get; init; }
    public required int GeneralTop3Count { get; init; }
    public required int GeneralTop1Count { get; init; }

    public required int CarA8Points { get; init; }
    public required int CarA8PointsTop5Count { get; init; }
    public required int CarA8PointsTop1Count { get; init; }

    public required int CarA7Points { get; init; }
    public required int CarA7PointsTop5Count { get; init; }
    public required int CarA7PointsTop1Count { get; init; }

    public required int CarA6Points { get; init; }
    public required int CarA6PointsTop5Count { get; init; }
    public required int CarA6PointsTop1Count { get; init; }

    public required int CarA5Points { get; init; }
    public required int CarA5PointsTop5Count { get; init; }
    public required int CarA5PointsTop1Count { get; init; }

    public required int CarBonusPoints { get; init; }
    public required int CarBonusPointsTop5Count { get; init; }
    public required int CarBonusPointsTop1Count { get; init; }


    public required int BonusPoints { get; init; }

    public Driver? Driver { get; set; }
}