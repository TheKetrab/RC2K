using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RC2K.DataAccess.Database;
using MudBlazor.Services;
using RC2K.Presentation.Blazor.ViewModels.Layout;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DataAccess.Dynamic.Repositories;
using RC2K.Logic.Fillers;
using RC2K.Logic.Interfaces.Fillers;
using RC2K.Logic.Interfaces;
using RC2K.Logic;
using Microsoft.Extensions.Caching.Memory;
using RC2K.DataAccess.Database.Cache;
using RC2K.DataAccess.Interfaces.Cache;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Database.Repositories;
using RC2K.Presentation.Blazor.AuthProxy;


namespace RC2K.Presentation.Blazor;

public static class BuilderConfiguration
{
    public static WebApplicationBuilder ConfigureRazor(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddHttpContextAccessor();

        return builder;
    }

    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddMudServices();

        builder
            .RegisterPersistentDataAccess()
            .RegisterDynamicDataAccess()
            .RegisterLogicServices();

        builder.Services.AddSingleton<HeaderViewModel>();

        return builder;
    }

    public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "auth_token";
                options.LoginPath = "/login";
                options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
                options.AccessDeniedPath = "/access-denied";
            });

        builder.Services.AddScoped<AuthService>();
        return builder;
    }

    public static WebApplicationBuilder RegisterDynamicDataAccess(this WebApplicationBuilder builder)
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

        builder.Services.AddScoped<BonusPointsMapper>();
        builder.Services.AddScoped<DriverMapper>();
        builder.Services.AddScoped<TimeEntryMapper>();
        builder.Services.AddScoped<UserMapper>();
        builder.Services.AddScoped<VerifyInfoMapper>();


        builder.Services.AddScoped<IBonusPointsRepository, BonusPointsRepository>();
        builder.Services.AddScoped<IDriverRepository, DriverRepository>();
        builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IVerifyInfoRepository, VerifyInfoRepository>();
        return builder;
    }

    public static WebApplicationBuilder RegisterLogicServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBonusPointsFiller, BonusPointsFiller>();
        builder.Services.AddScoped<ITimeEntryFiller, TimeEntryFiller>();
        builder.Services.AddScoped<IDriverFiller, DriverFiller>();
        builder.Services.AddScoped<IUserFiller, UserFiller>();
        builder.Services.AddScoped<IVerifyInfoFiller, VerifyInfoFiller>();

        builder.Services.AddScoped<IFillersBag, FillersBag>();
        builder.Services.AddScoped<IBonusPointsService, BonusPointsService>();
        builder.Services.AddScoped<IStageService, StageService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IDriverService, DriverService>();
        builder.Services.AddScoped<IPointsProvider, PointsProvider>();
        builder.Services.AddScoped<IPasswordProvider, PasswordProvider>(provider =>
        {
            var securitySection = builder.Configuration.GetSection("Security");
            int iterations = securitySection.GetValue<int>("Iterations");
            string salt = securitySection.GetValue<string>("Salt");

            return new PasswordProvider(iterations, salt);
        });
        builder.Services.AddScoped<ICarService, CarService>();

        builder.Services.AddScoped<TimeEntryService>();
        builder.Services.AddScoped<ITimeEntryService, AuthTimeEntryServiceProxy>();

        return builder;
    }

    public static WebApplicationBuilder RegisterPersistentDataAccess(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<RallyDbContext>();

        builder.Services.AddScoped<IMemoryCache, MemoryCache>();
        builder.Services.AddScoped<ICarCache, CarCache>();
        builder.Services.AddScoped<IStageCache, StageCache>();
        builder.Services.AddScoped<ICarRepository, CarRepository>();
        builder.Services.AddScoped<IStageRepository, StageRepository>();
        builder.Services.AddScoped<IRallyUoW, RallyUoW>();
        return builder;
    }

    public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<RallyDbContext>(opt =>
        {
            opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        return builder;
    }

    public static WebApplicationBuilder AddAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();
        builder.Services.AddCascadingAuthenticationState();

        return builder;
    }
}
 