using Microsoft.Extensions.Configuration;
using System.IO;

namespace Vecto.UWP
{
    /// <summary>
    /// Singleton class 
    /// </summary>
    public class AppSettings
    {
        public static AppSettings Instance { get; } = new AppSettings();

        public IConfiguration Configuration { get; protected set; }

        private AppSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public static string GetSectionString(string key) =>
            Instance.Configuration.GetSection(key).Value;
    }
}
