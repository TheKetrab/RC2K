using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RC2K.E2ETests.PageManagers;

namespace RC2K.E2ETests;

[TestFixture]
public class LoginTests
{
    private ChromeDriver _driver;
    private LoginPageManager _loginPageManager;
    private NavigationBarManager _navigationBarManager;

    [SetUp]
    public void SetUp()
    {        
        _driver = E2EFixture.CreateChromeDriver();
        _loginPageManager = new LoginPageManager(_driver);
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
    public void Login_UserCanLogin_AndSeesProfileBadge()
    {
        // login
        _navigationBarManager.NavigateToLogin();
        _loginPageManager.TypeUsername(E2EFixture.Config.TestUser);
        _loginPageManager.TypePassword(E2EFixture.Config.TestUserPassword);


        _loginPageManager.ClickLoginButton();

        // verify logged in
        IWebElement profileButton = _driver.FindElement(By.XPath("//button[@class='profile-btn']"));
        Assert.That(profileButton.Displayed, Is.True, "Profile button is not visible on the page.");
        profileButton.Click();

        // logoutBy
        IWebElement logoutButton = _driver.FindElement(
            By.XPath("//div[contains(@class,'profile-content-wrapper')]/*[last()][self::button]")
        );
        logoutButton.Click();

        // verify logged out
        Thread.Sleep(3000); // wait 3s to logout

        IWebElement profileButton2 = null;
        try
        {
            profileButton2 = _driver.FindElement(By.XPath("//button[@class='profile-btn']"));
        }
        catch (Exception) { }
        
        Assert.That(profileButton2, Is.Null, "Profile button is still visible on the page.");

    }

}
