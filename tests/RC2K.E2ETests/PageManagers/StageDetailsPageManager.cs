using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC2K.E2ETests.PageManagers;

public class StageDetailsPageManager(ChromeDriver driver)
{
    public IWebElement GetToSimulationButton()
    {
        return driver.FindElement(By.XPath("//button[contains(., 'To simulation')]"));
    }

    public IWebElement GetNextButton()
    {
        return driver.FindElement(By.XPath("//button[contains(., '>')]"));
    }

    public string GetStageTitle()
    {
        IWebElement stageCard = driver.FindElement(By.XPath("//div[contains(@class, 'card')][.//h3[text()='Stage details']]"));

        IWebElement stageTitleElement = stageCard.FindElement(By.XPath(".//div[contains(@class, 'card-body')]//h1"));

        string stageTitle = stageTitleElement.Text;
        return stageTitle;
    }
}
