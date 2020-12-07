using Ninject.Modules;
using Revo.Core.Core;
using Revo.Core.Lifecycle;
using Revo.Domain.Events;
using Revo.Infrastructure.Events.Async;
using Revo.MongoDB.Configuration;

namespace Revo.MongoDB.Projections
{
    [AutoLoadModule(false)]
    public class MongoProjectionsModule : NinjectModule
    {
        private readonly MongoConfigurationSection configurationSection;

        public MongoProjectionsModule(MongoConfigurationSection configurationSection)
        {
            this.configurationSection = configurationSection;
        }

        public override void Load()
        {
            Bind<IAsyncEventSequencer<DomainAggregateEvent>, MongoProjectionEventListener.MongoProjectionEventSequencer>()
                .To<MongoProjectionEventListener.MongoProjectionEventSequencer>()
                .InTaskScope();

            Bind<IAsyncEventListener<DomainAggregateEvent>>()
                .To<MongoProjectionEventListener>()
                .InTaskScope();

            Bind<IMongoProjectionSubSystem>()
                .To<MongoProjectionSubSystem>()
                .InTaskScope();

            if (configurationSection.AutoDiscoverProjectors)
            {
                Bind<IApplicationConfigurer>()
                    .To<MongoProjectorDiscovery>()
                    .InSingletonScope();
            }
        }
    }
}
