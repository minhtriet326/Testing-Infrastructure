using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Configuration
{
    public class DriverConfig
    {
        /// <summary>
        /// Gets or sets the type of browser driver (e.g., "Chrome", "Firefox").
        /// </summary>
        public required string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the browser should run in headless mode.
        /// </summary>
        public required bool Headless { get; set; }

        /// <summary>
        /// Gets or sets the width of the browser window.
        /// </summary>
        public required int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the browser window.
        /// </summary>
        public required int Height { get; set; }

        /// <summary>
        /// Gets or sets the list of arguments to be passed to the browser driver.
        /// </summary>
        public required List<string> Arguments { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the download path for the browser.
        /// </summary>
        public required string DownloadPath { get; set; }
    }
}
