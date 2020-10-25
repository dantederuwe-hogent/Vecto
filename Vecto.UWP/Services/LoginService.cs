using System;
using System.Net.Http;
using System.Threading.Tasks;
using Vecto.Application.Login;
using Vecto.UWP.Helpers;
using Windows.Security.Credentials;
using Windows.Storage;

namespace Vecto.UWP.Services
{
    public class LoginService
    {
        private readonly HttpClient _client;

        public LoginService()
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(AppSettings.GetSectionString("ApiBaseUrl"))
            };

        }
        public async Task Login(LoginDTO model, bool rememberMe)
        {
            var result = await _client.PostAsync("login", new JsonContent(model));
            var token = await result.Content.ReadAsStringAsync();
            token = token.Replace(@"""", ""); //TODO

            if (rememberMe) StorePassword(model);
            StoreToken(token);

            //TODO ERROR HANDLING
        }

        private static void StorePassword(LoginDTO model)
        {
            var vault = new PasswordVault();
            vault.Add(new PasswordCredential("Vecto", model.Email, model.Password));
        }

        private static void StoreToken(string token)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["token"] = token;
        }
    }
}
