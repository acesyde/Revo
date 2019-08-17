﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Revo.AspNetCore.Core
{
    public interface IAspNetCoreStartupConfigurer
    {
        void ConfigureServices(IServiceCollection services);
        void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory);
    }
}
