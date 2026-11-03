using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing_browser.Services.Dtos
{
    public class AuthResponse
    {
        public required string Token { get; set; }
    }

    public class AuthResponseContainer
    {
        public required AuthResponse Data { get; set; }
    }
}
