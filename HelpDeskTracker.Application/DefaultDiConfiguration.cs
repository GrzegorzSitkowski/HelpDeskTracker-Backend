using HelpDeskTracker.Application.Interfaces;
using HelpDeskTracker.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTracker.Application
{
    public static class DefaultDiConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentAccountProvider, CurrentAccountProvider>();

            return services;
        }
    }
}
