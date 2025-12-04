using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface IBonusPointsService
{
    Task<List<BonusPoints>> GetAll();
    Task Create(BonusPoints bonusPoints);
}
