using Microsoft.Extensions.Caching.Memory;
using RC2K.DataAccess.Interfaces.Cache;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Database.Cache;

public class StageCache(IMemoryCache cache) : GenericCache<Stage>(cache), IStageCache;
