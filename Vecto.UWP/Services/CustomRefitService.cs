using Refit;
using System;
using System.Net.Http;
using Windows.Storage;

namespace Vecto.UWP.Services
{
    public static class CustomRefitService
    {
        public static T For<T>() =>
            RestService.For<T>(AppSettings.GetSectionString("ApiBaseUrl"));

        public static T ForAuthenticated<T>()
        {
            var baseUri = new Uri(AppSettings.GetSectionString("ApiBaseUrl"));
            var tokenHandler = new TokenHandler(ApplicationData.Current.LocalSettings.Values["token"].ToString());

            var client = new HttpClient(tokenHandler) { BaseAddress = baseUri };

            return RestService.For<T>(client);
        }
    }
}