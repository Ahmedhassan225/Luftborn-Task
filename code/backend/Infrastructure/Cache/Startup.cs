using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Infrastructure.Cache
{
    internal static class Startup
    {

        internal static IServiceCollection AddCache(this IServiceCollection services)
        {
            return services
                .AddDistributedMemoryCache();    
        }
    }
}

