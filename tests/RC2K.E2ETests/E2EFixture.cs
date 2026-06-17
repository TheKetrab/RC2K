using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Chrome;

namespace RC2K.E2ETests;

[SetUpFixture]
public class E2EFixture
{
    public static Configuration Config { get; private set; }

    public static ChromeDriver CreateChromeDriver()
    {
        var options = new ChromeOptions();
        options.AddArgument("--no-sandbox");

        if (Config.ShowBrowser)
        {
            options.AddArgument("--start-maximized");
        }
        else
        {
            options.AddArgument("--headless=new");
        }

        var driverService = ChromeDriverService.CreateDefaultService(AppContext.BaseDirectory);
        driverService.EnableVerboseLogging = false;

        return new ChromeDriver(driverService, options);
    }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        SetupConfiguration();
        await VerifyAppIsReady();
    }

    private void SetupConfiguration()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<E2EFixture>()
            .AddEnvironmentVariables()
            .Build();

        Config = configuration.Get<Configuration>() ?? new Configuration();
    }

    private async Task VerifyAppIsReady()
    {
        var http = new HttpClient();
        var deadline = DateTime.UtcNow.AddSeconds(60);
        var success = false;
        while (DateTime.UtcNow < deadline)
        {
            try
            {
                var r = await http.GetAsync(Config.Host);
                if (r.IsSuccessStatusCode)
                {
                    success = true;
                    break;
                }
            }
            catch
            {

            }
            await Task.Delay(3000);
        }
        if (!success)
        {
            throw new TimeoutException("Blazor app did not start within 60s");
        }
    }
}
