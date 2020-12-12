using Refit;
using System.Threading.Tasks;
using Vecto.Application.Login;
using Vecto.Application.Profile;

namespace Vecto.UWP.Services
{
    public interface IApiService
    {
        [Post("/login")]
        Task<string> Login([Body]LoginDTO loginDTO);

        [Get("/profile")]
        Task<ProfileDTO> GetProfile();
    }
}