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
using RC2K.DataAccess.Database;
using RC2K.DataAccess.Database.Cache;
using RC2K.DataAccess.Database.Repositories;
using Microsoft.EntityFrameworkCore;

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
        builder.Services.AddSingleton<TimeEntryMapper>();
        builder.Services.AddSingleton<VerifyInfoMapper>();
        builder.Services.AddSingleton<IFillersBag, FillersBag>();

        builder.Services.AddSingleton<IDriverRepository, DriverRepository>();
        builder.Services.AddSingleton<IUserRepository, UserRepository>();
        builder.Services.AddSingleton<ITimeEntryRepository, TimeEntryRepository>();
        builder.Services.AddSingleton<IVerifyInfoRepository, VerifyInfoRepository>();


        builder.Services.AddSingleton<IUserFiller, UserFiller>();
        builder.Services.AddSingleton<IDriverFiller, DriverFiller>();
        builder.Services.AddSingleton<IVerifyInfoFiller, VerifyInfoFiller>();
        builder.Services.AddSingleton<ITimeEntryFiller, TimeEntryFiller>();
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<ICarService, CarService>();
        builder.Services.AddSingleton<IStageService, StageService>();
        builder.Services.AddSingleton<ITimeEntryService, TimeEntryService>();

        builder.Services.AddSingleton<IPasswordProvider, PasswordProvider>(provider =>
        {
            var securitySection = builder.Configuration.GetSection("Security");
            int iterations = securitySection.GetValue<int>("Iterations");
            string salt = securitySection.GetValue<string>("Salt");

            return new PasswordProvider(iterations, salt);
        });


        builder.Services.AddSingleton<RallyDbContext>();

        builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
        builder.Services.AddSingleton<ICarCache, CarCache>();
        builder.Services.AddSingleton<IStageCache, StageCache>();
        builder.Services.AddSingleton<ICarRepository, CarRepository>();
        builder.Services.AddSingleton<IStageRepository, StageRepository>();
        builder.Services.AddSingleton<IRallyUoW, RallyUoW>();
       
        builder.Services.AddDbContext<RallyDbContext>(opt =>
        {
            opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
        }, optionsLifetime: ServiceLifetime.Singleton);


        return builder;
    }

}
