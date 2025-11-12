using Microsoft.Extensions.Caching.Memory;
using RC2K.DataAccess.Interfaces.Cache;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DataAccess.Interfaces;
using RC2K.Logic.Fillers;
using RC2K.Logic.Interfaces.Fillers;
using RC2K.Logic.Interfaces;
using RC2K.Logic;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Repositories;

namespace RC2K.WebApi;

public static class CompositionRoot
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        var cosmosSection = builder.Configuration.GetSection("Cosmos");

        string endpoint = cosmosSection.GetValue<string>("Endpoint");
        string database = cosmosSection.GetValue<string>("Database");
        string primaryKey = cosmosSection.GetValue<string>("ApiKey");

        CosmosClient client =
            new CosmosClientBuilder(endpoint, primaryKey)
                .WithSystemTextJsonSerializerOptions(new System.Text.Json.JsonSerializerOptions())
                .Build();

        Database db = client.GetDatabase(database);
        builder.Services.AddSingleton(db);

        builder.Services.AddSingleton<DriverMapper>();
        builder.Services.AddSingleton<UserMapper>();

        builder.Services.AddSingleton<IDriverRepository, DriverRepository>();
        builder.Services.AddSingleton<IUserRepository, UserRepository>();


        builder.Services.AddSingleton<IUserFiller, UserFiller>();
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<IPasswordProvider, PasswordProvider>();

        return builder;
    }

}
