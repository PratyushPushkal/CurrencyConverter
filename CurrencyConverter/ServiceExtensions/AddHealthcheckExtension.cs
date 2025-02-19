using CurrencyConverter.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CurrencyConverter.WebAPI.ServiceExtensions
{
    public static class AddHealthcheckExtension
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddUrlGroup(new Uri(Common.FrankfuterUrl), failureStatus : HealthStatus.Unhealthy);
            return services;
        }
    }
}
