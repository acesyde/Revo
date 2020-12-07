using Ninject;
using Revo.Core.Types;
using Revo.Infrastructure.Projections;

namespace Revo.MongoDB.Projections
{
    public class MongoProjectorDiscovery : ProjectorDiscovery
    {
        public MongoProjectorDiscovery(ITypeExplorer typeExplorer, StandardKernel kernel)
            : base(typeExplorer, kernel, new[] { typeof(IMongoEntityEventProjector<>) })
        {
        }
    }
}
