using core.Logging.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testing_browser.Utils;

namespace testing_browser.WebDriver
{
    /// <summary>
    /// Provides an abstraction layer over <see cref="IWebDriver"/> for interacting with web pages and elements,
    /// enriched with logging and wait support.
    /// </summary>
    public class WebBrowser
    {
        /// <summary>
        /// Gets the raw Selenium WebDriver instance.
        /// </summary>
        public IWebDriver RawDriver { get; private set; }

        /// <summary>
        /// Gets the WebDriverWait instance used for explicit waits.
        /// </summary>
        public WebDriverWait Wait { get; private set; }

        private ITestLogger logger;

        /// <summary>
        /// Returns a new <see cref="Actions"/> instance scoped to the current driver session.
        /// </summary>
        protected Actions actions => GetActions(); //should use fresh actions

        public WebBrowser(IWebDriver rawDriver, ITestLogger logger, int timeoutInSeconds = 10)
        {
            RawDriver = rawDriver;
            this.logger = logger;
            Wait = RawDriver.MakeDriverWait(timeoutInSeconds);
        }

        /// <summary>
        /// Navigates the browser to the specified URL.
        /// </summary>
        public void NavigateTo(string url)
        {
            RawDriver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Switches focus to the active browser alert.
        /// </summary>
        public IAlert SwitchToAlert()
        {
            return RawDriver.SwitchTo().Alert();
        }

        // <summary>
        /// Gets a new <see cref="Actions"/> instance.
        /// </summary>
        public Actions GetActions()
        {
            return new Actions(RawDriver);
        }

        /// <summary>
        /// Finds a visible element using a specified locator and returns a wrapped <see cref="WebElement"/>.
        /// </summary>
        public WebElement Find(By locator)
        {
            logger.LogInfo($"Finding element {locator}.");

            var element = RawDriver.WaitUntilExist(locator);

            return new WebElement(RawDriver, element, locator, logger);
        }

        /// <summary>
        /// Finds all matching elements using a specified locator and wraps them in <see cref="WebElement"/> objects.
        /// </summary>
        public List<WebElement> Finds(By locator)
        {
            logger.LogInfo($"Finding elements {locator}.");

            var elements = RawDriver.FindElements(locator);

            return elements.Select(e => new WebElement(RawDriver, e, locator, logger)).ToList();
        }

        /// <summary>
        /// Finds an element without using explicit waits.
        /// </summary>
        public WebElement FindWithouWaiting(By locator)
        {
            logger.LogInfo($"Finding without waiting element {locator}.");

            var element = RawDriver.FindElement(locator);
            
            return new WebElement(RawDriver, element, locator, logger);
        }

        /// <summary>
        /// Performs a drag-and-drop operation from one element to another.
        /// </summary>
        public void DragAndDrop(WebElement source, WebElement target)
        {
            logger.LogInfo("Perform drag and drop.");

            actions
                .ClickAndHold(source.Element)
                .Pause(TimeSpan.FromMilliseconds(500))
                .MoveToElement(target.Element)
                .Release()
                .Pause(TimeSpan.FromMilliseconds(500))
                .Perform();
        }

        /// <summary>
        /// Sets the size of the browser window to the specified dimensions.
        /// </summary>
        public void SetWindowSize(Size size)
        {
            logger.LogInfo($"Set window size with width: {size.Width} and height: {size.Height}.");

            RawDriver.Manage().Window.Size = size;
        }

        /// <summary>
        /// Quits the browser and disposes of the WebDriver instance.
        /// </summary>
        public void Quit()
        {
            RawDriver.Quit();
        }

        /// <summary>
        /// Adds or updates a session cookie in the browser with the specified name and value.
        /// </summary>
        /// <remarks>
        /// This method deletes any existing cookie with the same name before adding the new one.
        /// The cookie is set with the path "/" and no expiration, making it a session cookie.
        /// </remarks>
        public void AddCookie(string cookieName, string cookieValue)
        {
            // phải NavigateTo tới website trước rồi mới AddCookie
            // tạo Cookie
            var cookie = new Cookie(
                name: cookieName,
                value: cookieValue,
                path: "/" // áp dụng cookie này cho mọi page (dấu "/")
                );// không set hết hạn => nên đây là session cookie: tồn tại đến khi đóng trình duyệt thì biến mất

            // xóa cookie cùng tên 
            RawDriver.Manage().Cookies.DeleteCookieNamed(cookieName);

            // thêm cookie
            RawDriver.Manage().Cookies.AddCookie(cookie);
        }

        /// <summary>
        /// Retrieves a specific cookie from the browser by its name.
        /// </summary>
        public Cookie? GetCookie(string cookieName) => RawDriver.Manage().Cookies.GetCookieNamed(cookieName);

        /// <summary>
        /// Executes JavaScript
        /// </summary>
        public void ExecuteScript(string script)
        {
            logger.LogInfo($"Execute script {script}.");

            ((IJavaScriptExecutor)RawDriver).ExecuteScript(script);
        }

        /// <summary>
        /// Executes JavaScript and returns a typed result.
        /// </summary>
        public T? ExecuteScript<T>(string script)
        {
            logger.LogInfo($"Execute script {script}.");

            return (T?)((IJavaScriptExecutor)RawDriver).ExecuteScript(script);
        }

        /// <summary>
        /// Captures a screenshot of the current browser window and saves it to log.
        /// </summary>
        public void Screenshot()
        {
            var screenshot = ((ITakesScreenshot)RawDriver).GetScreenshot();

            logger.LogScreenshot(screenshot.AsBase64EncodedString);
        }
    }
}
