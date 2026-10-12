using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Configuration
{
    public class ApiConfig
    {
        /// <summary>
        /// Gets or sets the base URL for the API. (e.g., "https://login.pearson.com/v1/piapi/iesui/signin")
        /// </summary>
        public required string BaseUrl { get; set; }
    }
}
