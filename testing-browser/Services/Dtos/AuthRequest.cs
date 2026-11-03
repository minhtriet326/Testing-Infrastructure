using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing_browser.Services.Dtos
{
    public class AuthRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
