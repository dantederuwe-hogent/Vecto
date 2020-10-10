using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vecto.Core.Interfaces;
using Vecto.Infrastructure.Data;
using Vecto.Infrastructure.Data.Repositories;

namespace Vecto.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("VectoDB")));

            var provider = services.BuildServiceProvider();
            var context = provider.GetRequiredService<AppDbContext>();
            context.Database.EnsureDeleted(); //TODO DEV ONLY
            context.Database.EnsureCreated();


            services.AddScoped<IUserRepository, UserRepository>();
        }
    }

}
