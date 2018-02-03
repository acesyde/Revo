﻿using Revo.Platforms.AspNet.Boostrap;
using Microsoft.Owin;
using Owin;
using Revo.Platforms.AspNet.Core;
using Revo.Platforms.AspNet.Core.Lifecycle;

[assembly: OwinStartup(typeof(Startup))]
namespace Revo.Platforms.AspNet.Boostrap
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            foreach (IOwinConfigurator owinConfigurator in RevoHttpApplication.Current.ResolveAll<IOwinConfigurator>())
            {
                owinConfigurator.ConfigureApp(app);
            }
        }
    }
}