using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess.Database;
using RC2K.DataAccess.Dynamic;
using RC2K.DataAccess.Dynamic.EnvironmentProviders;
using RC2K.Logic;

namespace RC2K.IntegrationTests;

[SetUpFixture]
public class IntegrationFixture
{
    private static IServiceProvider? _serviceProvider;
    public static IServiceProvider ServiceProvider => _serviceProvider ?? throw new Exception();

    [OneTimeSetUp]
    public void OneTimeSetup()
    {

        try
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<IntegrationFixture>()
                .Build();

            IServiceCollection services = new ServiceCollection()
                .RegisterPersistentDataAccess(s => { })
                .RegisterDynamicDataAccess(configuration, typeof(DevEnvironmentProvider))
                .RegisterLogicServices(configuration);

            _serviceProvider = services.BuildServiceProvider();
        }
        catch (Exception ex)
        {
            
        }

        
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
