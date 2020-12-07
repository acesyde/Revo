using Revo.Domain.Entities;
using Revo.Infrastructure.Projections;
using Revo.MongoDB.DataAccess;

namespace Revo.MongoDB.Projections
{
    public class  MongoEntityEventToPocoProjector<TSource, TTarget> :
        CrudEntityEventToPocoProjector<TSource, TTarget>,
        IMongoEntityEventProjector<TSource>
        where TSource : class, IAggregateRoot
        where TTarget : class, new()
    {
        public MongoEntityEventToPocoProjector(IMongoCrudRepository repository) : base(repository)
        {
            Repository = repository;
        }

        protected new IMongoCrudRepository Repository { get; }
    }
}
