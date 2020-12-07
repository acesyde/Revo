using Revo.Domain.Entities;
using Revo.Infrastructure.Projections;

namespace Revo.MongoDB.Projections
{
    public interface IMongoEntityEventProjector<T> : IEntityEventProjector
        where T : IAggregateRoot
    {
    }
}
