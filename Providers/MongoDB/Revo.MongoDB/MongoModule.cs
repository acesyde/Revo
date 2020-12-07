using Ninject.Modules;
using Revo.Core.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Ninject;
using Revo.DataAccess.Entities;
using Revo.MongoDB.Configuration;
using Revo.MongoDB.DataAccess;

namespace Revo.MongoDB
{
    [AutoLoadModule(false)]
    public class MongoModule : NinjectModule
    {
        private readonly MongoConnectionConfiguration connectionConfiguration;
        private readonly bool useAsPrimaryRepository;

        public MongoModule(MongoConnectionConfiguration connectionConfiguration, bool useAsPrimaryRepository)
        {
            this.connectionConfiguration = connectionConfiguration;
            this.useAsPrimaryRepository = useAsPrimaryRepository;
        }

        public override void Load()
        {
            Bind<IMongoClient>()
                .ToMethod(ctx =>
                {
                    var connectionParams = connectionConfiguration?.ConnectionString?
                        .Split(';')
                        .Select(m => new MongoServerAddress(m));

                    var store = new MongoClient(new MongoClientSettings
                    {
                        Servers = connectionParams
                    });

                    return store;

                }).InSingletonScope();

            Bind<IMongoDatabase>()
                .ToMethod(ctx =>
                {
                    var documentStore = ctx.Kernel.Get<IMongoClient>();
                    return documentStore.GetDatabase(connectionConfiguration.Database);
                })
                .InTaskScope();

            List<Type> repositoryTypes = new List<Type>()
            {
                typeof(IMongoCrudRepository)
            };

            if (useAsPrimaryRepository)
            {
                repositoryTypes.AddRange(new[]
                {
                    typeof(ICrudRepository), typeof(IReadRepository)
                });
            }

            Bind(repositoryTypes.ToArray())
                .To<IMongoCrudRepository>()
                .InTaskScope();

            Bind(repositoryTypes.Select(x => typeof(ICrudRepositoryFactory<>).MakeGenericType(x)).ToArray())
                .To<MongoCrudRepositoryFactory>()
                .InTaskScope();
        }
    }
}
