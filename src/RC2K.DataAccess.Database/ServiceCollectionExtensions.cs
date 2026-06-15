using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess.Database.Cache;
using RC2K.DataAccess.Database.Repositories;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Cache;
using RC2K.DataAccess.Interfaces.Repositories;

namespace RC2K.DataAccess.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterPersistentDataAccess(
        this IServiceCollection services, Action<IServiceCollection> addDbContext)
    {
        addDbContext(services);

        services.AddSingleton<IMemoryCache, MemoryCache>();
        services.AddSingleton<ICarCache, CarCache>();
        services.AddSingleton<IStageCache, StageCache>();

        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IStageRepository, StageRepository>();
        services.AddScoped<IRallyUoW, RallyUoW>();

        return services;
    }
}
