using FTD.M3.API.Services;
using FTD.M3.API.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FTD.M3.API.Helpers
{
    public static class IocConfiguration
    {
        public static void ServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IM3Service, M3RequestService>();
        }
    }
}
