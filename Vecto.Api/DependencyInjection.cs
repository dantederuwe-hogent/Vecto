using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vecto.Api
{
    public static class DependencyInjection
    {
        public static void AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddOpenApiDocument(s =>
            {
                s.DocumentName = "apidocs";
                s.Title = "Vecto API";
                s.Version = "v1";
                s.Description = "The API for Vecto. Built by Dante De Ruwe and Liam Spitaels.";
                /*
                s.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                s.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT")); //adds the token when a request is send

                */
            });
        }
    }
}
