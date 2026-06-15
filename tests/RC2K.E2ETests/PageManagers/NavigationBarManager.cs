using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace RC2K.E2ETests.PageManagers;

public class NavigationBarManager(ChromeDriver driver)
{
    public void NavigateToStages()
    {
        IWebElement stagesButton = driver.FindElement(By.CssSelector("a[href='/stages']"));

        stagesButton.Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.Url.Contains("/stages"));
    }
}
