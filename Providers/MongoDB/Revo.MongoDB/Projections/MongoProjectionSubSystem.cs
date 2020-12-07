using Revo.Core.Core;
using Revo.Domain.Entities;
using Revo.Infrastructure.Events;
using Revo.Infrastructure.Projections;
using Revo.MongoDB.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revo.MongoDB.Projections
{
    public class MongoProjectionSubSystem : ProjectionSubSystem, IMongoProjectionSubSystem
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IMongoCrudRepository repository;

        public MongoProjectionSubSystem(IEntityTypeManager entityTypeManager,
            IEventMessageFactory eventMessageFactory, IServiceLocator serviceLocator,
            IMongoCrudRepository repository) : base(entityTypeManager, eventMessageFactory)
        {
            this.serviceLocator = serviceLocator;
            this.repository = repository;
        }

        protected override IEnumerable<IEntityEventProjector> GetProjectors(Type entityType, EventProjectionOptions options)
        {
            return serviceLocator.GetAll(
                    typeof(IMongoEntityEventProjector<>).MakeGenericType(entityType))
                .Cast<IEntityEventProjector>();
        }

        protected override async Task CommitUsedProjectorsAsync(IReadOnlyCollection<IEntityEventProjector> usedProjectors, EventProjectionOptions options)
        {
            await base.CommitUsedProjectorsAsync(usedProjectors, options);
            await repository.SaveChangesAsync();
        }
    }
}
