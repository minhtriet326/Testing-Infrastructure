using core.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testing_browser.Services.Dtos;
using testing_browser.Services.Interfaces;

namespace testing_browser.Services
{
    public class AuthService : IAuthService
    {
        private RestClient client;
        public AuthService(ApiConfig config)
        {
            client = new RestClient(config.BaseUrl);
        }
        public async Task<AuthResponse> LoginAsync(string username, string password, bool rememberMe = false)
        {
            var requestBody = new AuthRequest{Username = username, Password = password};

            var restRequest = new RestRequest("/api/auth/login", Method.Post);

            restRequest.AddJsonBody(requestBody);

            var response = await client.ExecuteAsync<AuthResponseContainer>(restRequest);

            if (response.StatusCode != System.Net.HttpStatusCode.OK || response.Data == null)
            {
                throw new Exception($"Login failed: {response.Content}");
            }

            return response.Data.Data;
        }
    }
}
