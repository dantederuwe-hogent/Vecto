using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vecto.Application.Login;
using Vecto.Application.Profile;
using Vecto.Application.Register;

namespace Vecto.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IValidator<LoginDTO>, LoginValidator>();
            services.AddTransient<IValidator<RegisterDTO>, RegisterValidator>();
            services.AddTransient<IValidator<ProfileDTO>, ProfileValidator>();
        }
    }
}