﻿using System.Collections.Generic;
using GTRevo.Core.Core.Lifecycle;
using GTRevo.Platform.Core;
using Ninject.Modules;
using PushSharp.Google;

namespace GTRevo.Infrastructure.Notifications.Channels.Fcm
{
    public class FcmNotificationsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IFcmBrokerDispatcher, IApplicationStartListener>()
                .ToMethod(ctx =>
                {
                    var configSection = LocalConfiguration.Current
                        .GetSection<FcmServiceConfigurationSection>(
                            FcmServiceConfigurationSection.ConfigurationSectionName);

                    List<FcmAppConfiguration> configs = new List<FcmAppConfiguration>();

                    if (configSection != null)
                    {
                        for (int i = 0; i < configSection.AppConfigurations.Count; i++)
                        {
                            var configElement = configSection.AppConfigurations[i];
                            configs.Add(new FcmAppConfiguration(configElement.AppId,
                                new GcmConfiguration(configElement.SenderAuthToken)));
                        }
                    }

                    return new FcmBrokerDispatcher(configs);
                })
                .InSingletonScope();
        }
    }
}
