using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RC2K.E2ETests.PageManagers;

namespace RC2K.E2ETests;

[TestFixture]
public class StageDetailsTests
{
    private ChromeDriver _driver;
    private StageDetailsPageManager _stageDetailsPageManager;
    private NavigationBarManager _navigationBarManager;

    [SetUp]
    public void SetUp()
    {
        _driver = E2EFixture.CreateChromeDriver();
        _stageDetailsPageManager = new StageDetailsPageManager(_driver);
        _navigationBarManager = new NavigationBarManager(_driver);

        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        _driver.Navigate().GoToUrl(E2EFixture.Config.Host);
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
