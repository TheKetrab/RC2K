using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface IRankingsRepository
{
    Task<RankingSnapshot> GetCurrent();
}
