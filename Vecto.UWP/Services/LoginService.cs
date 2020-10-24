using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Vecto.Application.Login;
using Vecto.UWP.Helpers;

namespace Vecto.UWP.Services
{
    public class LoginService
    {
        private readonly HttpClient _client;

        public LoginService()
        {
            _client = new HttpClient() { BaseAddress = new Uri(AppSettings.GetSectionString("ApiBaseUrl")) };

        }
        public async Task Login(LoginDTO model)
        {
            var result = await _client.PostAsync("login", new JsonContent(model));
            var token = await result.Content.ReadAsStringAsync();
            Debug.Write(token);
            //TODO STORE TOKEN
            //TODO ERROR HANDLING
        }
    }
}
