using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Mappers;

public class BonusPointsMapper : IModelMapper<BonusPoints, BonusPointsModel>
{
    public BonusPointsModel ToCosmosModel(BonusPoints bonusPoints)
    {
        BonusPointsModel model = new BonusPointsModel()
        {
            Id = bonusPoints.Id,
            DriverId = bonusPoints.DriverId,
            Comment = bonusPoints.Comment,
            Points = bonusPoints.Points
        };

        return model;
    }

    public BonusPoints ToDomainModel(BonusPointsModel model)
    {
        BonusPoints bonusPoints = new BonusPoints()
        {
            Id = model.Id,
            DriverId = model.DriverId,
            Comment = model.Comment,
            Points = model.Points
        };

        return bonusPoints;
    }
}
