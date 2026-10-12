using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Configuration
{
    public class AuthConfig
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Url { get; set; }
    }
}
