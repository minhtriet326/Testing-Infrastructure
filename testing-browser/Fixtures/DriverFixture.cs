using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testing_browser.WebDriver;

namespace testing_browser.Fixtures
{
    /// <summary>
    /// Provides a shared <see cref="DriverFactory"/> instance for use in UI tests,
    /// typically used with xUnit's <c>IClassFixture</c>.
    /// </summary>
    public class DriverFixture
    {
        /// <summary>
        /// Gets the <see cref="DriverFactory"/> instance used to create WebDriver instances.
        /// </summary>
        public DriverFactory DriverFactory { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverFixture"/> class.
        /// </summary>
        public DriverFixture()
        {
            DriverFactory = new DriverFactory();
        }
    }
}
