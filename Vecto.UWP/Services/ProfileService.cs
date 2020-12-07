using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Vecto.Application.Profile;
using Windows.Storage;

namespace Vecto.UWP.Services
{
    public class ProfileService
    {
        private readonly HttpClient _client;
        private string token;

        public ProfileService()
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(AppSettings.GetSectionString("ApiBaseUrl"))
            };
            token = ApplicationData.Current.LocalSettings.Values["token"].ToString();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }

        public async Task<ProfileDTO> GetProfile()
        {
            var result = await _client.GetAsync("profile");
            return JsonConvert.DeserializeObject<ProfileDTO>(await result.Content.ReadAsStringAsync());
        }
    }
}
