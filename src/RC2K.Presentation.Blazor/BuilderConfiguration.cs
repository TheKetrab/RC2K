using Blazor.Analytics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MudBlazor.Services;
using RC2K.DataAccess.Database;
using RC2K.DataAccess.Dynamic;
using RC2K.DataAccess.Dynamic.EnvironmentProviders;
using RC2K.DomainModel.Exceptions;
using RC2K.Logic;
using RC2K.Logic.Interfaces;
using RC2K.Parser;
using RC2K.Presentation.Blazor.AuthProxy;
using RC2K.Presentation.Blazor.TrafficLimits;
using RC2K.Presentation.Blazor.Views;
using RC2K.Presentation.Blazor.Views.Dialogs;
using Serilog;
using Serilog.Exceptions;

namespace RC2K.Presentation.Blazor;

public static class BuilderConfiguration
{
    public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    /// <summary>
    /// Configures entire ASP.NET & Blazor system with authentication & authorization
    /// </summary>
    public static WebApplicationBuilder ConfigureRazor(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.ConfigureLogging();
        builder.ConfigureIdentity();

        builder.Services.AddHealthChecks();

        builder.Services.AddHttpClient();

        if (builder.Configuration["ApplicationInsights:ConnectionString"] is not null)
        {
            builder.Services.AddSingleton<ITelemetryClientWrapper, TelemetryClientWrapper>();
            builder.Services.AddApplicationInsightsTelemetry(options =>
            {
                var conn = builder.Configuration["ApplicationInsights:ConnectionString"];
                if (!string.IsNullOrEmpty(conn))
                {
                    options.ConnectionString = conn;
                }
            });
        }
        else
        {
            builder.Services.AddSingleton<ITelemetryClientWrapper, TelemetryClientNullObjectWrapper>();
        }

        builder.Services.AddSingleton<ActiveSessionTracker>();
        builder.Services.AddScoped<CircuitHandler, TrackingCircuitHandler>();
        builder.Services.AddIdleCircuitHandler(options => 
        {
            options.IdleTimeout = TimeSpan.FromMinutes(3);
            options.ForceInvalidateCircuitTimeout = TimeSpan.FromSeconds(10);
        });
        builder.Services.AddHostedService<SessionLoggingService>();

        builder.Services.AddMudServices();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<DialogHelper>();
        builder.Services.AddScoped<MessageHelper>();
        builder.Services.AddSingleton<Shared.ViewModels.HeaderViewModel>();

        return builder;
    }

    /// <summary>
    /// Configures DI for application
    /// </summary>
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        string? conn = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.RegisterPersistentDataAccess(
            services => services.AddDbContext<RallyDbContext>(opt => opt.UseSqlite(conn)));

        builder.Services.RegisterDynamicDataAccess(builder.Configuration,
            builder.Configuration["ASPNETCORE_ENVIRONMENT"] switch
            {
                "Development" => typeof(DevEnvironmentProvider),
                "Production" => typeof(ProdEnvironmentProvider),
                _ => throw new ArgumentException(
                    $"Unknown environment: {builder.Configuration["ASPNETCORE_ENVIRONMENT"]}"
                    + " (Set up proper env var ASPNETCORE_ENVIRONMENT")
            });

        builder.Services
            .RegisterLogicServices(builder.Configuration)
            .OverrideAuthServices();

        builder.Services.AddScoped<IHstReader, HstReader>();
        builder.Services.AddScoped<IHstUploadManager, HstUploadManager>();
        builder.Services.AddScoped<IMailProvider, GmailProvider>(provider =>
        {
            var mailingSection = builder.Configuration.GetSection("Mailing");
            string sftpAppPassword = mailingSection["SftpAppPassword"]
                ?? throw new MissingConfigurationKeyException("Mailing:SftpAppPassword");
            string senderEmail = mailingSection["SenderEmail"]
                ?? throw new MissingConfigurationKeyException("Mailing:SenderEmail");

            return new GmailProvider(senderEmail, sftpAppPassword);
        });

        builder.Services.AddScoped<ICaptchaVerifier, ReCaptchaV3Verifier>(provider =>
        {
            var captchaSection = builder.Configuration.GetSection("Captcha");
            string secretKey = captchaSection["SecretKey"]
                ?? throw new InvalidOperationException("Captcha:SecretKey is not configured.");
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var logger = provider.GetRequiredService<ILogger<ReCaptchaV3Verifier>>();

            return new ReCaptchaV3Verifier(secretKey, httpClientFactory, logger);
        });

        return builder;
    }

    /// <summary>
    /// Configures logging
    /// </summary>
    private static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        string analyticsId = builder.Configuration["Analytics:GoogleAnalyticsId"]
            ?? throw new InvalidOperationException("Analytics:GoogleAnalyticsId is not configured.");
        builder.Services.AddGoogleAnalytics(analyticsId);

        builder.Host.UseSerilog((ctx, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.WithExceptionDetails()
            .Enrich.WithEnvironmentName()
            .WriteTo.Console());

        return builder;
    }

    /// <summary>
    /// Configures authentication & authorization
    /// </summary>
    private static WebApplicationBuilder ConfigureIdentity(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "auth_token";
                options.LoginPath = "/login";
                options.Cookie.MaxAge = TimeSpan.FromDays(30);
                options.AccessDeniedPath = "/access-denied";
            });

        builder.Services.AddScoped<AuthService>();

        builder.Services.AddCors(options =>
            options.AddPolicy(
                name: MyAllowSpecificOrigins,
                policy =>
                {
                    string[] allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                        ?? throw new InvalidOperationException("Cors:AllowedOrigins is not configured.");
                    policy.WithOrigins(allowedOrigins);
                })
        );

        builder.Services.AddAuthorization();
        builder.Services.AddCascadingAuthenticationState();

        return builder;
    }

    /// <summary>
    /// Adds authorization to logic services
    /// </summary>
    private static IServiceCollection OverrideAuthServices(this IServiceCollection services)
    {
        services.AddScoped<TimeEntryService>();
        services.AddScoped<ITimeEntryService, AuthTimeEntryServiceProxy>();

        services.AddScoped<NotificationService>();
        services.AddScoped<INotificationService, AuthNotificationServiceProxy>();

        services.AddScoped<RankingService>();
        services.AddScoped<IRankingService, AuthRankingServiceProxy>();

        services.AddScoped<UserService>();
        services.AddScoped<IUserService, AuthUserServiceProxy>();

        return services;
    }
}
 