using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface IBonusPointsRepository
{
    Task<List<BonusPoints>> GetAll();
    Task Create(BonusPoints bonusPoints);
}
