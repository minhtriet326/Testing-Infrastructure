using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Configuration
{
    public class TestConfig
    {
        /// <summary>
        /// Gets or sets the browser driver configuration settings.
        /// </summary>
        public required DriverConfig Driver { get; set; }

        /// <summary>
        /// Gets or sets the API configuration settings used for making HTTP requests.
        /// </summary>
        public required ApiConfig Api { get; set; }

        public required AuthConfig Auth { get; set; }
    }
}
