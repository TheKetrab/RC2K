using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RC2K.DomainModel.Exceptions;
using RC2K.Logic.Fillers;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterLogicServices(this IServiceCollection services, IConfiguration configuration)
    {
        // fillers
        services.AddScoped<IBonusPointsFiller, BonusPointsFiller>();
        services.AddScoped<ITimeEntryFiller, TimeEntryFiller>();
        services.AddScoped<IDriverFiller, DriverFiller>();
        services.AddScoped<IUserFiller, UserFiller>();
        services.AddScoped<IVerifyInfoFiller, VerifyInfoFiller>();
        services.AddScoped<IRankingFiller, RankingFiller>();
        services.AddScoped<IFillersBag, FillersBag>();

        // services
        services.AddScoped<IBonusPointsService, BonusPointsService>();
        services.AddScoped<IStageService, StageService>();
        services.AddScoped<IDriverService, DriverService>();
        services.AddScoped<IPointsProvider, PointsProvider>();
        services.AddScoped<IPasswordProvider, PasswordProvider>(provider =>
        {
            var securitySection = configuration.GetSection("Security");
            int iterations = securitySection.GetIntValue("Iterations")
                ?? throw new MissingConfigurationKeyException("Security:Iterations");
            string salt = securitySection["Salt"]
                ?? throw new MissingConfigurationKeyException("Security:Salt");

            return new PasswordProvider(iterations, salt);
        });
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<ITimeEntryService, TimeEntryService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IRankingService, RankingService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    private static int? GetIntValue(this IConfigurationSection configurationSection, string key)
    {
        if (int.TryParse(configurationSection[key], out int res))
        {
            return res;
        }

        return null;
    }
}
