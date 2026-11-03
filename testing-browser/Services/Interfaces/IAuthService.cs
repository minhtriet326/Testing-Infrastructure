using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testing_browser.Services.Dtos;

namespace testing_browser.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthResponse> LoginAsync(string username, string password, bool rememberMe = false);
    }
}
