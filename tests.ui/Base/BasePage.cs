using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testing_browser.Utils;
using testing_browser.WebDriver;

namespace tests.ui.Base
{
    /// <summary>
    /// Defines a shared test collection for E2E test cases, enabling the reuse of common fixtures:
    /// <see cref="ConfigFixture"/>, <see cref="LoggingFixture"/>, and <see cref="DriverFixture"/>.
    /// </summary>
    public abstract class BasePage
    {
        /// <summary>
        /// The browser driver used to interact with the page.
        /// </summary>
        protected WebBrowser driver;

        /// <summary>
        /// Gets the default URL to navigate to before the test starts.
        /// </summary>
        /// <remarks>
        /// This property can be overridden in derived test classes to specify the starting page for the test.
        /// By default, it returns an empty string, indicating no automatic navigation.
        /// </remarks>
        protected virtual string Url => "";

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePage"/> class with the given browser driver.
        /// </summary>
        /// <param name="driver">The <see cref="WebBrowser"/> instance to use for page interaction.</param>
        public BasePage(WebBrowser driver)
        {
            this.driver = driver;
        }

        /// <summary>
        /// Waits for the current page to be fully loaded and ready.
        /// </summary>
        public void WaitForPageReady() => driver.WaitForPageReady(); // extension method

        /// <summary>
        /// Navigates the browser to the default URL defined by the <c>Url</c> property.
        /// </summary>
        /// <remarks>
        /// This method uses the configured <c>driver</c> to navigate to the page specified by the
        /// <c>Url</c> property. Override <c>Url</c> in derived classes to customize the destination.
        /// </remarks>
        public void NavigateTo() => driver.NavigateTo(Url);
    }
}
