using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Repositories;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel.Exceptions;

namespace RC2K.DataAccess.Dynamic;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDynamicDataAccess(
        this IServiceCollection services, IConfiguration configuration, Type environmentProviderType)
    {
        var cosmosSection = configuration.GetSection("Cosmos");

        string endpoint = cosmosSection["Endpoint"]
            ?? throw new MissingConfigurationKeyException("Cosmos:Endpoint");
        string database = cosmosSection["Database"]
            ?? throw new MissingConfigurationKeyException("Cosmos:Database");
        string primaryKey = cosmosSection["ApiKey"]
            ?? throw new MissingConfigurationKeyException("Cosmos:ApiKey");
        CosmosClient client =
            new CosmosClientBuilder(endpoint, primaryKey)
                .WithSystemTextJsonSerializerOptions(new System.Text.Json.JsonSerializerOptions())
                .Build();

        Database db = client.GetDatabase(database);
        services.AddSingleton(db);

        services.AddScoped<BonusPointsMapper>();
        services.AddScoped<CronMessageMapper>();
        services.AddScoped<DriverMapper>();
        services.AddScoped<NotificationMapper>();
        services.AddScoped<TimeEntryMapper>();
        services.AddScoped<RankingsMapper>();
        services.AddScoped<UserMapper>();
        services.AddScoped<VerifyInfoMapper>();

        services.AddScoped(typeof(IEnvironmentProvider), environmentProviderType);

        services.AddScoped<IBonusPointsRepository, BonusPointsRepository>();
        services.AddScoped<ICronMessageRepository, CronMessageRepository>();
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();
        services.AddScoped<IRankingsRepository, RankingsRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVerifyInfoRepository, VerifyInfoRepository>();
        
        return services;
    }
}
