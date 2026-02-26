using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Caching.Memory;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Repositories;

public class RankingsRepository(Database database, RankingsMapper mapper, IEnvironmentProvider envProvider, IMemoryCache cache)
    : CosmosRepository<RankingSnapshot, RankingSnapshotModel, RankingsMapper>(database, mapper, envProvider)
    , IRankingsRepository
{
    private const string RankingSnapshotKey = "RankingSnapshotKey";
    public override string EntityName => "Statistics";

    public async Task<RankingSnapshot> GetCurrent()
    {
        if (cache.TryGetValue<RankingSnapshot>(RankingSnapshotKey, out var cachedSnapshot))
        {
            return cachedSnapshot!;
        }

        var query = new QueryDefinition(@"
            SELECT * FROM c WHERE c.type = 'ranking' ORDER BY c._ts DESC");

        var lst = await FetchAll(query);
        var activeSnapshot = lst.First();

        cache.Set(RankingSnapshotKey, activeSnapshot);

        return activeSnapshot;
    }
}
