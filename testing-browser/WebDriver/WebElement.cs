using core.Logging.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing_browser.WebDriver
{
    public class WebElement
    {
        public IWebDriver Driver { get; private set; }
        public IWebElement Element { get; private set; }
        public By Locator { get; private set; }
        public ITestLogger Logger { get; private set; }
        public WebElement(IWebDriver driver, IWebElement element, By locator, ITestLogger logger)
        {
            Driver = driver;
            Element = element;
            Locator = locator;
            Logger = logger;
        }

        /// <summary>
        /// Clicks the element by scrolling it into view first.
        /// </summary>
        public void Click()
        {
            ScrollIntoViewAndClick();
        }

        /// <summary>
        /// Clears the element's value.
        /// </summary>
        public void Clear() => Element.Clear();

        /// <summary>
        /// Types the specified text into the element.
        /// </summary>
        public void Type(string text)
        {
            Logger.LogInfo($"Typing text '{text}' into element located by {Locator}.");
            ScrollIntoView();
            Element.SendKeys(text);
        }

        /// <summary>
        /// Selects a value from a dropdown using the "value" attribute.
        /// </summary>
        public void SelectByValue(string value)
        {
            Logger.LogInfo($"Selecting option with value '{value}' in element located by {Locator}.");
            ScrollIntoView();
            SelectElement selectElement = new SelectElement(Element);
            selectElement.SelectByValue(value);
        }

        /// <summary>
        /// Selects a value from a dropdown using the visible text.
        /// </summary>
        public void SelectByText(string text)
        {
            Logger.LogInfo($"Selecting option with text '{text}' in element located by {Locator}.");
            ScrollIntoView();
            SelectElement selectElement = new SelectElement(Element);
            selectElement.SelectByText(text);
        }

        public void ScrollIntoViewAndClick() 
        {
            Logger.LogInfo($"Clicking on {Locator}.");
            ScrollIntoView();
            Element.Click();
        }

        /// <summary>
        /// Scrolls the element into the visible area of the browser window.
        /// </summary>
        public void ScrollIntoView()
        {
            Logger.LogInfo($"Scrolling element located by {Locator} into view.");
            ExecuteScript("arguments[0].scrollIntoView(behavior: 'auto', block: 'center', inline: 'nearest');");
        }

        /// <summary>
        /// Executes JavaScript with the current element as an argument.
        /// </summary>
        public void ExecuteScript(string script)
        {
            Logger.LogInfo($"Executing script {script}.");
            ((IJavaScriptExecutor)Driver).ExecuteScript(script, Element);
        }

        /// <summary>
        /// Click regardless of whether the element is clickable or not
        /// </summary>
        public void ClickByJs()
        {
            Logger.LogInfo($"Click By Js {Element}");
        }

        /// <summary>
        /// Checks whether the element is valid according to the browser's validation rules.
        /// </summary>
        public bool? IsValid()
        {
            return (bool?)((IJavaScriptExecutor)Driver).ExecuteScript(
                "return arguments[0].checkValidity()", 
                Element);
        }

        /// <summary>
        /// Gets the value of the "class" attribute.
        /// </summary>
        public string? GetClass() => Element.GetAttribute("class");

        /// <summary>
        /// Gets the value of a specific CSS property.
        /// </summary>
        public string? GetCssValue(string css) => Element.GetCssValue(css);

        /// <summary>
        /// Gets the inner text of the element.
        /// </summary>
        public string Text => Element.Text;

        /// <summary>
        /// Indicates whether the element is enabled.
        /// </summary>
        public bool Enabled() => Element.Enabled;

        /// <summary>
        /// Indicates whether the element is displayed.
        /// </summary>
        public bool Displayed => Element.Displayed;

        /// <summary>
        /// Gets the value of a specified attribute.
        /// </summary>
        public string? GetAttribute(string property) => Element.GetAttribute(property);

        /// <summary>
        /// Finds a child element relative to the current element using the specified locator,
        /// waiting until it exists or the timeout is reached.
        /// </summary>
        public WebElement Find(By locator, int timeOutInSeconds = 10)
        {
            Logger.LogInfo($"Finding element {locator} inside {this.Locator}");

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOutInSeconds));

            var element = wait.Until(Driver =>
            {
                try
                {
                    return this.Element.FindElement(locator);
                }
                catch (NoSuchElementException ex)
                {
                    return null;
                }
            });

            return new WebElement(this.Driver, element, locator, this.Logger);
        }

        /// <summary>
        /// Finds all child elements that match the specified locator within the current element.
        /// </summary>
        public List<WebElement> Finds(By locator)
        {
            Logger.LogInfo($"Finding elements with locator: {locator} inside {this.Locator}.");

            List<IWebElement> result = this.Element.FindElements(locator).ToList();

            return result.Select(e => new WebElement(this.Driver, e, locator, this.Logger)).ToList();
        }

        /// <summary>
        /// Creates a new <see cref="WebElement"/> instance using the specified Selenium <see cref="IWebElement"/> as the new source,
        /// while preserving the current driver, locator, and logger context.
        /// </summary>
        public WebElement CreateWithNewElement(IWebElement newElement)
        {
            return new WebElement(this.Driver, newElement, this.Locator, this.Logger);
        }
    }
}
