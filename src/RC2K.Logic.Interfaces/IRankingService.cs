using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface IRankingService
{
    Task<RankingSnapshot> GetLatest();
}
