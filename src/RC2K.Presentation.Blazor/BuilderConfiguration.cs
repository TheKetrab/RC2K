
using Blazor.Analytics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MudBlazor.Services;
using RC2K.DataAccess.Database;
using RC2K.DataAccess.Database.Cache;
using RC2K.DataAccess.Database.Repositories;
using RC2K.DataAccess.Dynamic.EnvironmentProviders;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Repositories;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Cache;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.Logic;
using RC2K.Logic.Fillers;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Fillers;
using RC2K.Parser;
using RC2K.Presentation.Blazor.AuthProxy;
using RC2K.Presentation.Blazor.ViewModels;
using Serilog;
using Serilog.Exceptions;


namespace RC2K.Presentation.Blazor;

public static class BuilderConfiguration
{

    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Services.AddGoogleAnalytics("G-TBW7LHNXJF");

        builder.Host.UseSerilog((ctx, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.WithExceptionDetails()
            .Enrich.WithEnvironmentName()
            .WriteTo.Console());

        return builder;
    }
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

        builder.Services.AddSingleton<Shared.ViewModels.HeaderViewModel>();

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
        builder.Services.AddScoped<NotificationMapper>();
        builder.Services.AddScoped<TimeEntryMapper>();
        builder.Services.AddScoped<RankingsMapper>();
        builder.Services.AddScoped<UserMapper>();
        builder.Services.AddScoped<VerifyInfoMapper>();

        builder.Services.AddScoped(
            typeof(IEnvironmentProvider),
            builder.Configuration["ASPNETCORE_ENVIRONMENT"] switch
            {
                "Development" => typeof(DevEnvironmentProvider),
                "Production" => typeof(ProdEnvironmentProvider),
                _ => throw new Exception(
                    $"Unknown environment: {builder.Configuration["ASPNETCORE_ENVIRONMENT"]}"
                    + " (Set up proper env var ASPNETCORE_ENVIRONMENT")
            });

        builder.Services.AddScoped<IBonusPointsRepository, BonusPointsRepository>();
        builder.Services.AddScoped<IDriverRepository, DriverRepository>();
        builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
        builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();
        builder.Services.AddScoped<IRankingsRepository, RankingsRepository>();
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
        builder.Services.AddScoped<IRankingFiller, RankingFiller>();
        builder.Services.AddScoped<IHstReader, HstReader>();
        builder.Services.AddScoped<IHstUploadManager, HstUploadManager>();
        builder.Services.AddScoped<IFillersBag, FillersBag>();
        builder.Services.AddScoped<IBonusPointsService, BonusPointsService>();
        builder.Services.AddScoped<IStageService, StageService>();
        builder.Services.AddScoped<IDriverService, DriverService>();
        builder.Services.AddScoped<IPointsProvider, PointsProvider>();
        builder.Services.AddScoped<IPasswordProvider, PasswordProvider>(provider =>
        {
            var securitySection = builder.Configuration.GetSection("Security");
            int iterations = securitySection.GetValue<int>("Iterations");
            string salt = securitySection.GetValue<string>("Salt");

            return new PasswordProvider(iterations, salt);
        });
        builder.Services.AddScoped<IMailProvider, GmailProvider>(provider =>
        {
            var mailingSection = builder.Configuration.GetSection("Mailing");
            string sftpAppPassword = mailingSection.GetValue<string>("SftpAppPassword");
            string senderEmail = mailingSection.GetValue<string>("SenderEmail");

            return new GmailProvider(senderEmail, sftpAppPassword);
        });
        builder.Services.AddScoped<ICaptchaVerifier, ReCaptchaV3Verifier>(provider =>
        {
            var captchaSection = builder.Configuration.GetSection("Captcha");
            string secretKey = captchaSection.GetValue<string>("SecretKey");
            var logger = provider.GetRequiredService<ILogger<ReCaptchaV3Verifier>>();

            return new ReCaptchaV3Verifier(secretKey, logger);
        });

        builder.Services.AddScoped<ICarService, CarService>();

        builder.Services.AddScoped<TimeEntryService>();
        builder.Services.AddScoped<ITimeEntryService, AuthTimeEntryServiceProxy>();

        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddScoped<INotificationService, AuthNotificationServiceProxy>();

        builder.Services.AddScoped<RankingService>();
        builder.Services.AddScoped<IRankingService, AuthRankingServiceProxy>();

        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<IUserService, AuthUserServiceProxy>();

        return builder;
    }

    public static WebApplicationBuilder RegisterPersistentDataAccess(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<RallyDbContext>();

        builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
        builder.Services.AddSingleton<ICarCache, CarCache>();
        builder.Services.AddSingleton<IStageCache, StageCache>();

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

    public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    public static WebApplicationBuilder AddAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                name: MyAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins(
                        "https://mango-sky-047fada03-featrc2k67static.westeurope.4.azurestaticapps.net", // feature branch featrc2k67
                        "https://mango-sky-047fada03.4.azurestaticapps.net", // prod/main
                        "https://rc2khub.com",
                        "https://www.rc2khub.com");
                });
        });

        builder.Services.AddAuthorization();
        builder.Services.AddCascadingAuthenticationState();

        return builder;
    }
}
 