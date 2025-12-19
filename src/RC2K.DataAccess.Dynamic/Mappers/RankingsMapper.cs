using RC2K.DataAccess.Dynamic.Models;
using static RC2K.Utils.Utils;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Mappers;

public class RankingsMapper : IModelMapper<RankingSnapshot, RankingSnapshotModel>
{
    public RankingSnapshot ToDomainModel(RankingSnapshotModel model)
    {
        RankingSnapshot rankingSnapshot = new()
        {
            Id = model.Id,
            Date = StringToDateTime(model.Date)
        };

        foreach (var entry in model.Entries)
        {
            int[] points = entry.Data.Split(",").Select(int.Parse).ToArray();
            rankingSnapshot.Entries.Add(new RankingEntry()
            {
                Place = entry.Place,
                DriverId = entry.DriverId,

                GeneralPoints = points[0],
                GeneralTop30Count = points[1],
                GeneralTop10Count = points[2],
                GeneralTop3Count = points[3],
                GeneralTop1Count = points[4],
                CarA8Points = points[5],
                CarA8PointsTop5Count = points[6],
                CarA8PointsTop1Count = points[7],
                CarA7Points = points[8],
                CarA7PointsTop5Count = points[9],
                CarA7PointsTop1Count = points[10],
                CarA6Points = points[11],
                CarA6PointsTop5Count = points[12],
                CarA6PointsTop1Count = points[13],
                CarA5Points = points[14],
                CarA5PointsTop5Count = points[15],
                CarA5PointsTop1Count = points[16],
                CarBonusPoints = points[17],
                CarBonusPointsTop5Count = points[18],
                CarBonusPointsTop1Count = points[19],
                BonusPoints = points[20],
            });
        }

        return rankingSnapshot;
    }

    public RankingSnapshotModel ToCosmosModel(RankingSnapshot rankingSnapshot)
    {
        RankingSnapshotModel model = new()
        {
            Id = rankingSnapshot.Id,
            Date = DateTimeToString(rankingSnapshot.Date)
        };

        foreach (var entry in rankingSnapshot.Entries)
        {
            string data = string.Join(",",
            [
                entry.GeneralPoints,
                entry.GeneralTop30Count,
                entry.GeneralTop10Count,
                entry.GeneralTop3Count,
                entry.GeneralTop1Count,
                entry.CarA8Points,
                entry.CarA8PointsTop5Count,
                entry.CarA8PointsTop1Count,
                entry.CarA7Points,
                entry.CarA7PointsTop5Count,
                entry.CarA7PointsTop1Count,
                entry.CarA6Points,
                entry.CarA6PointsTop5Count,
                entry.CarA6PointsTop1Count,
                entry.CarA5Points,
                entry.CarA5PointsTop5Count,
                entry.CarA5PointsTop1Count,
                entry.CarBonusPoints,
                entry.CarBonusPointsTop5Count,
                entry.CarBonusPointsTop1Count,
                entry.BonusPoints,
            ]);

            model.Entries.Add(new RankingEntryModel()
            {
                Place = entry.Place,
                DriverId = entry.DriverId,
                Data = data
            });
            
        }

        return model;
    }
}
