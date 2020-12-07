using Revo.Core.Configuration;
using System;
using Revo.MongoDB.Projections;

namespace Revo.MongoDB.Configuration
{
    public static class MongoConfigurationExtensions
    {
        public static IRevoConfiguration UseMongoDataAccess(this IRevoConfiguration configuration,
            MongoConnectionConfiguration connection,
            bool? useAsPrimaryRepository = true,
            Action<MongoConfigurationSection> advancedAction = null)
        {
            var section = configuration.GetSection<MongoConfigurationSection>();
            section.IsActive = true;
            section.Connection = connection ?? section.Connection;
            section.UseAsPrimaryRepository = useAsPrimaryRepository ?? section.UseAsPrimaryRepository;

            advancedAction?.Invoke(section);

            configuration.ConfigureKernel(c =>
            {
                if (section.IsActive)
                {
                    c.LoadModule(new MongoModule(section.Connection, section.UseAsPrimaryRepository));
                }
            });

            return configuration;
        }

        public static IRevoConfiguration UseMongoProjections(this IRevoConfiguration configuration,
            bool autoDiscoverProjectors = true,
            Action<MongoConfigurationSection> advancedAction = null)
        {
            configuration.UseMongoDataAccess(null);

            var section = configuration.GetSection<MongoConfigurationSection>();
            section.UseProjections = true;
            section.AutoDiscoverProjectors = autoDiscoverProjectors;

            advancedAction?.Invoke(section);

            configuration.ConfigureKernel(c =>
            {
                if (section.UseProjections)
                {
                    c.LoadModule(new MongoProjectionsModule(section));
                }
            });

            return configuration;
        }
    }
}
