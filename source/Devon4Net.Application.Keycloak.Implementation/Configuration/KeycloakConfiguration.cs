using Devon4Net.Application.Keycloak.Implementation.Options;
using Devon4Net.Application.Keycloak.Implementation.Services;
using Devon4Net.Infrastructure.Common.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Devon4Net.Application.Keycloak.Implementation.Configuration
{
    public static class KeycloakConfiguration
    {
        public static void SetupKeycloakService(this IServiceCollection services, IConfiguration configuration)
        {
            var keycloakOptions = services.GetTypedOptions<KeycloakOptions>(configuration, "Keycloak");

            if (keycloakOptions == null) return;

            services.AddSingleton(new KeycloakService(keycloakOptions));
        }

        public static void SetupKeycloakAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var keycloakOptions = services.GetTypedOptions<KeycloakOptions>(configuration, "Keycloak");

            services.AddAuthorization(options =>
            {
                options.AddPolicy("SchoolPolicy", policy => policy.RequireClaim("user_roles", "Administrator"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>
            {
                o.Authority = keycloakOptions.Authority;
                o.Audience = keycloakOptions.Audience;
                o.SaveToken = true;
                o.RequireHttpsMetadata = false;
#if DEBUG
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();

                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";

                        Console.WriteLine(c.Exception.ToString());

                        return c.Response.WriteAsync("An error occured processing your authentication.");
                    }
                };
#endif
            });
        }

        public static void SetupKeycloakAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
