using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testing_browser.WebDriver;
using WebElement = testing_browser.WebDriver.WebElement;

namespace testing_browser.Utils
{
    public static class WaitHelper
    {
        /// <summary>
        /// Creates a <see cref="WebDriverWait"/> instance for the specified <see cref="WebDriver"/>.
        /// </summary>
        public static WebDriverWait MakeDriverWait(this WebBrowser driver, int timeoutInSeconds = 10)
        {
            return new WebDriverWait(driver.RawDriver, TimeSpan.FromSeconds(timeoutInSeconds));
        }

        public static IWebElement WaitUntilExist(
            this WebBrowser driver, 
            By locator,
            int timeoutInSeconds = 10)
        {
            var localWait = MakeDriverWait(driver, timeoutInSeconds);
            return localWait.Until(d => d.FindElement(locator));
        }

        public static IWebElement WaitUntilVisible(
            this WebBrowser driver,
            By locator,
            int timeoutInSeconds = 10)
        {
            var localWait = MakeDriverWait(driver, timeoutInSeconds);
            return localWait.Until(d =>
            {
                try
                {
                    var element = d.FindElement(locator);
                    if (!element.Displayed)
                    {
                        return null;
                    }

                    return element;
                }
                catch (StaleElementReferenceException)
                {
                    // DOM vừa bị render lại -> coi như chưa visible, thử lại vòng sau
                    return null;
                }
            });
        }

        public static WebElement WaitUntilVisible(
            this WebBrowser driver,
            WebElement webElement,
            int timeoutInSeconds = 10)
        {
            var localWait = MakeDriverWait(driver, timeoutInSeconds);
            var visibleElement = localWait.Until(d =>
            {
                try
                {
                    var element = d.FindElement(webElement.Locator);
                    if (!element.Displayed)
                    {
                        return null;
                    }

                    return element;
                }
                catch (StaleElementReferenceException)
                {
                    // DOM vừa bị render lại -> coi như chưa visible, thử lại vòng sau
                    return null;
                }
            });

            return webElement.CreateWithNewElement(visibleElement);
        }

        public static bool WaitUntilNotVisible(
            this WebBrowser driver,
            WebElement webElement,
            int timeoutInSeconds = 10)
        {
            var localWait = MakeDriverWait(driver, timeoutInSeconds);
            return localWait.Until(d =>
            {
                try
                {
                    return !d.FindElement(webElement.Locator).Displayed;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            }); 
        }

        public static WebElement WaitUntilClickable(
            this WebBrowser driver,
            WebElement webElement,
            int timeoutInSeconds = 10)
        {
            var localWait = MakeDriverWait(driver, timeoutInSeconds);
            IWebElement element = localWait.Until(d =>
            {
                try
                {
                    var clickableElement = d.FindElement(webElement.Locator);
                    return (clickableElement.Displayed && clickableElement.Enabled) ? clickableElement : null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });

            return webElement.CreateWithNewElement(element);
        }

        public static IAlert WaitForAlert(
            this WebBrowser driver,
            int timeoutInSeconds = 10)
        {
            var localWait = MakeDriverWait(driver, timeoutInSeconds);
            return localWait.Until(d =>
            {
                try
                {
                    return d.SwitchTo().Alert();
                }
                catch (NoAlertPresentException)
                {
                    return null;
                }
            });
        }

        public static void WaitForPageReady(
            this WebBrowser driver,
            int timeoutInSeconds = 30)
        {
            var localWait = MakeDriverWait(driver, timeoutInSeconds);
            localWait.Until(d =>
            {
                var js = (IJavaScriptExecutor)d;

                var result = js.ExecuteScript(
                    "return document.readyState === 'complete' && " +
                    "(typeof jQuery === 'undefined' || jQuery.active === 0)");

                return (bool?)result ?? false;
            });
        }
    }
}
