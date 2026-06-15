using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RC2K.E2ETests.PageManagers;

namespace RC2K.E2ETests;

[TestFixture]
public class StageDetailsTests
{
    private const string Host = "http://app.rc2khub.com";
    private ChromeDriver _driver;
    private StageDetailsPageManager _stageDetailsPageManager;
    private NavigationBarManager _navigationBarManager;


    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var http = new HttpClient();
        var deadline = DateTime.UtcNow.AddSeconds(60);
        var success = false;
        while (DateTime.UtcNow < deadline)
        {
            try
            {
                var r = await http.GetAsync(Host);
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


    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless=new");
        options.AddArgument("--no-sandbox");
        //options.AddArgument("--start-maximized");

        var driverService = ChromeDriverService.CreateDefaultService(AppContext.BaseDirectory);
        driverService.EnableVerboseLogging = false;

        _driver = new ChromeDriver(driverService, options);
        _stageDetailsPageManager = new StageDetailsPageManager(_driver);
        _navigationBarManager = new NavigationBarManager(_driver);

        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        _driver.Navigate().GoToUrl(Host);
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    [Test]
    public void StageDetailsNavigation_UserUsesNextAndSwitchModeButton_ReloadsTitle()
    {
        _navigationBarManager.NavigateToStages();

        IWebElement ballagyr = _driver.FindElement(By.LinkText("Ballagyr (A)"));
        Assert.That(ballagyr, Is.Not.Null, "Could not find Ballagyr row");
        ballagyr.Click();

        _stageDetailsPageManager.GetToSimulationButton().Click();

        _stageDetailsPageManager.GetNextButton().Click();

        var title = _stageDetailsPageManager.GetStageTitle();
        Assert.That(title, Is.EqualTo("Curraghs"));
    }

}
