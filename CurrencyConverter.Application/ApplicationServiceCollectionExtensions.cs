using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyConverter.Application.Interface;
using Microsoft.Extensions.DependencyInjection;
using CurrencyConverter.Application.Service;

namespace CurrencyConverter.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyConverterService, CurrencyConverterService>();
            return services;
        }
    }
}
