using DamianTourBackend.Application.Register;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vecto.Application.DTOs;
using Vecto.Application.Login;

namespace Vecto.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IValidator<LoginDTO>, LoginValidator>();
            services.AddTransient<IValidator<RegisterDTO>, RegisterValidator>();
        }
    }
}