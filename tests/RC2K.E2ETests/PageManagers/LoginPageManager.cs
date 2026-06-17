using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace RC2K.E2ETests.PageManagers;

public class LoginPageManager(ChromeDriver driver)
{
    public void TypeUsername(string username)
    {
        IWebElement usernameField = driver.FindElement(By.XPath("//label[text()='Username']/preceding-sibling::div//input"));
        usernameField.Clear();
        usernameField.SendKeys(username);
    }

    public void TypePassword(string password)
    {
        IWebElement usernameField = driver.FindElement(By.XPath("//label[text()='Password']/preceding-sibling::div//input"));
        usernameField.Clear();
        usernameField.SendKeys(password);
    }

    public void ClickLoginButton()
    {
        IWebElement loginButton = driver.FindElement(By.XPath("//button[.//span[text()='Login']]"));
        loginButton.Click();
    }
}
