using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace CurrencyConverter.WebAPI.ServiceExtensions
{
    public static class AddJwtServiceExtension
    {
        public static IServiceCollection AddJwtToken(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
                    ValidAudience = configuration.GetValue<string>("Jwt:Issuer"),
                    IssuerSigningKey = new SymmetricSecurityKey(
                                        Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key")))
                };
            });
            return services;
        }
    }
}
