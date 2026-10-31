using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing_browser.Utils
{
    public static class WaitHelper
    {
        public static WebDriverWait MakeDriverWait(this IWebDriver webDriver, int timeoutInSeconds = 10)
        {
            return new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeoutInSeconds));
        }

        public static IWebElement WaitUntilExist(
            this IWebDriver driver, 
            By locator,
            int timeoutInSeconds = 10)
        {
            var localWait = MakeDriverWait(driver, timeoutInSeconds);
            return localWait.Until(driver => driver.FindElement(locator));
        }
    }
}
