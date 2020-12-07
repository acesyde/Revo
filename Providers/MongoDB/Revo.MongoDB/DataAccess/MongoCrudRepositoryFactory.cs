using MongoDB.Driver;
using Revo.DataAccess.Entities;

namespace Revo.MongoDB.DataAccess
{
    public class MongoCrudRepositoryFactory :
        ICrudRepositoryFactory<IMongoCrudRepository>,
        ICrudRepositoryFactory<ICrudRepository>,
        ICrudRepositoryFactory<IReadRepository>
    {
        private readonly IMongoDatabase asyncDocumentSession;

        public MongoCrudRepositoryFactory(IMongoDatabase asyncDocumentSession)
        {
            this.asyncDocumentSession = asyncDocumentSession;
        }

        public IMongoCrudRepository Create()
        {
            return new MongoCrudRepository(asyncDocumentSession);
        }

        ICrudRepository ICrudRepositoryFactory<ICrudRepository>.Create()
        {
            return Create();
        }

        IReadRepository ICrudRepositoryFactory<IReadRepository>.Create()
        {
            return Create();
        }
    }
}
