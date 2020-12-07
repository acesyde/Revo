using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Revo.DataAccess.Entities;

namespace Revo.MongoDB.DataAccess
{
    public class MongoCrudRepository : IMongoCrudRepository
    {
        private readonly IMongoDatabase mongoDatabase;
        private readonly HashSet<object> addedEntities = new HashSet<object>();

        public MongoCrudRepository(IMongoDatabase mongoDatabase)
        {
            this.mongoDatabase = mongoDatabase;
        }

        public void Dispose()
        {
        }

        public IEnumerable<IRepositoryFilter> DefaultFilters { get; }

        public T Get<T>(object[] id) where T : class
        {
            throw new NotImplementedException();
        }

        public T Get<T>(object id) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(params object[] id) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(CancellationToken cancellationToken, params object[] id) where T : class
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync<T>(object id) where T : class
        {
            var builder = Builders<T>.Filter;
            builder.Eq("DocumentId", GetMongoId<T>(id.ToString()));
            T t = await GetMongoCollection<T>().Find(builder.OfType<T>()).FirstAsync();
            RepositoryHelpers.ThrowIfGetFailed<T>(t, id);
            return t;
        }

        public async Task<T> GetAsync<T>(CancellationToken cancellationToken, object id) where T : class
        {
            var builder = Builders<T>.Filter;
            builder.Eq("DocumentId", GetMongoId<T>(id.ToString()));
            T t = await GetMongoCollection<T>().Find(builder.OfType<T>()).FirstAsync(cancellationToken);
            RepositoryHelpers.ThrowIfGetFailed<T>(t, id);
            return t;
        }

        public Task<T[]> GetManyAsync<T, TId>(params TId[] ids) where T : class, IHasId<TId>
        {
            return GetManyAsync<T, TId>(default(CancellationToken), ids);
        }

        public async Task<T[]> GetManyAsync<T, TId>(CancellationToken cancellationToken, params TId[] ids) where T : class, IHasId<TId>
        {
            var result = await FindManyAsync<T, TId>(default(CancellationToken), ids);
            RepositoryHelpers.ThrowIfGetManyFailed(result, ids);
            return result;
        }

        public async Task<T[]> GetManyAsync<T>(params Guid[] ids) where T : class, IHasId<Guid>
        {
            var result = await FindManyAsync<T, Guid>(default, ids);
            RepositoryHelpers.ThrowIfGetManyFailed(result, ids);
            return result;
        }

        public async Task<T[]> GetManyAsync<T>(CancellationToken cancellationToken, params Guid[] ids) where T : class, IHasId<Guid>
        {
            var result = await FindManyAsync<T, Guid>(cancellationToken, ids);
            RepositoryHelpers.ThrowIfGetManyFailed(result, ids);
            return result;
        }

        public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            throw new NotImplementedException();
        }

        public T First<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) where T : class
        {
            return GetMongoCollection<T>().AsQueryable().Where(predicate).FirstOrDefaultAsync(cancellationToken);
        }

        public Task<T> FirstAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) where T : class
        {
            return GetMongoCollection<T>().AsQueryable().Where(predicate).FirstAsync(cancellationToken);
        }

        public T Find<T>(object[] id) where T : class
        {
            throw new NotImplementedException();
        }

        public T Find<T>(object id) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> FindAsync<T>(params object[] id) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> FindAsync<T>(CancellationToken cancellationToken, params object[] id) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> FindAsync<T>(object id) where T : class
        {
            var builder = Builders<T>.Filter;
            builder.Eq("DocumentId", GetMongoId<T>(id.ToString()));
            return GetMongoCollection<T>().Find(builder.OfType<T>()).FirstAsync();
        }

        public Task<T> FindAsync<T>(CancellationToken cancellationToken, object id) where T : class
        {
            var builder = Builders<T>.Filter;
            builder.Eq("DocumentId", GetMongoId<T>(id.ToString()));
            return GetMongoCollection<T>().Find(builder.OfType<T>()).FirstAsync(cancellationToken);
        }

        public IQueryable<T> FindAll<T>() where T : class
        {
            return GetMongoCollection<T>().AsQueryable();
        }

        public async Task<T[]> FindAllAsync<T>(CancellationToken cancellationToken) where T : class
        {
            var result = await GetMongoCollection<T>().AsQueryable().ToListAsync(cancellationToken);
            return result.ToArray();
        }

        public IEnumerable<T> FindAllWithAdded<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T[]> FindManyAsync<T, TId>(params TId[] ids) where T : class, IHasId<TId>
        {
            return FindManyAsync<T, TId>(default(CancellationToken), ids);
        }

        public Task<T[]> FindManyAsync<T, TId>(CancellationToken cancellationToken, params TId[] ids) where T : class, IHasId<TId>
        {
            return Task.FromResult(Where<T>(x => ids.Contains(x.Id)).ToArray());
        }

        public Task<T[]> FindManyAsync<T>(params Guid[] ids) where T : class, IHasId<Guid>
        {
            return FindManyAsync<T, Guid>(default(CancellationToken), ids);
        }

        public Task<T[]> FindManyAsync<T>(CancellationToken cancellationToken, params Guid[] ids) where T : class, IHasId<Guid>
        {
            return FindManyAsync<T, Guid>(cancellationToken, ids);
        }

        public IQueryable<T> Where<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return GetMongoCollection<T>().AsQueryable().Where(predicate);
        }

        public IEnumerable<T> WhereWithAdded<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetEntities<T>(params EntityState[] entityStates) where T : class
        {
            throw new NotImplementedException();
        }

        public EntityState GetEntityState<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public void SetEntityState<T>(T entity, EntityState state) where T : class
        {
            throw new NotImplementedException();
        }

        public bool IsAttached<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public void Attach<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public void AttachRange<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }

        public void Add<T>(T entity) where T : class
        {
            addedEntities.Add(entity);
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ((IMongoQueryable<T>)queryable).AnyAsync(cancellationToken);
        }

        public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ((IMongoQueryable<T>)queryable).CountAsync(cancellationToken);
        }

        public IQueryable<T> Include<T, TProperty>(IQueryable<T> queryable, Expression<Func<T, TProperty>> navigationPropertyPath) where T : class
        {
            throw new NotImplementedException();
            //var objectProperty = Expression.Lambda<Func<T, object>>(
            //    Expression.Convert(navigationPropertyPath.Body, typeof(object)),
            //    navigationPropertyPath.Parameters[0]);
            //return ((IMongoQueryable<T>)queryable).Include(objectProperty);
        }

        public IQueryable<T> Include<T>(IQueryable<T> queryable, string navigationPropertyPath) where T : class
        {
            throw new NotImplementedException();
            //return ((IMongoQueryable<T>)queryable).Include(navigationPropertyPath);
        }

        public Task<long> LongCountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ((IMongoQueryable<T>)queryable).LongCountAsync(cancellationToken);
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ((IMongoQueryable<T>)queryable).FirstOrDefaultAsync(cancellationToken);
        }

        public Task<T> FirstAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ((IMongoQueryable<T>)queryable).FirstAsync(cancellationToken);
        }

        public async Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await ((IMongoQueryable<T>)queryable).ToListAsync(cancellationToken)).ToArray();
        }

        public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ((IMongoQueryable<T>)queryable).ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<TKey, T>> ToDictionaryAsync<T, TKey>(IQueryable<T> queryable, Func<T, TKey> keySelector,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await ((IMongoQueryable<T>)queryable).ToListAsync(cancellationToken)).ToDictionary(keySelector);
        }

        public async Task<Dictionary<TKey, T>> ToDictionaryAsync<T, TKey>(IQueryable<T> queryable, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await ((IMongoQueryable<T>)queryable).ToListAsync(cancellationToken)).ToDictionary(keySelector, comparer);
        }

        public async Task<Dictionary<TKey, TElement>> ToDictionaryAsync<T, TKey, TElement>(IQueryable<T> queryable, Func<T, TKey> keySelector, Func<T, TElement> elementSelector,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await ((IMongoQueryable<T>)queryable).ToListAsync(cancellationToken)).ToDictionary(keySelector, elementSelector);
        }

        public async Task<Dictionary<TKey, TElement>> ToDictionaryAsync<T, TKey, TElement>(IQueryable<T> queryable, Func<T, TKey> keySelector, Func<T, TElement> elementSelector,
            IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await ((IMongoQueryable<T>)queryable).ToListAsync(cancellationToken)).ToDictionary(keySelector, elementSelector, comparer);
        }

        public void Remove<T>(T entity) where T : class
        {
            var builder = Builders<T>.Filter;
            builder.Eq("DocumentId", GetMongoId(entity));
            GetMongoCollection<T>().DeleteOne(builder.OfType<T>());
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {

            try
            {
                using (var session = await mongoDatabase.Client.StartSessionAsync(cancellationToken: cancellationToken))
                {
                    var transactionOptions = new TransactionOptions(
                        readPreference: ReadPreference.Primary,
                        readConcern: ReadConcern.Local,
                        writeConcern: WriteConcern.WMajority);

                    session.WithTransaction(
                        (s, ct) =>
                        {
                            foreach (object entity in addedEntities)
                            {
                                var bsonDocument = BsonDocument.Create(entity);
                                GetMongoCollection(entity).InsertOneAsync(bsonDocument, cancellationToken: cancellationToken);
                            }

                            return "Inserted into collections in different databases";
                        },
                        transactionOptions,
                        cancellationToken);
                }
            }
            catch (Exception e)
            {

                throw new OptimisticConcurrencyException($"Optimistic concurrency exception occurred while saving Mongo repository", e);
            }
        }

        protected IMongoCollection<T> GetMongoCollection<T>() => mongoDatabase.GetCollection<T>(typeof(T).FullName);
        protected IMongoCollection<BsonDocument> GetMongoCollection(object entity) => mongoDatabase.GetCollection<BsonDocument>(entity.GetType().FullName);

        protected string GetMongoId<T>(T entity)
        {
            if (entity is IHasId<Guid> hasGuid)
            {
                return GetMongoId(entity.GetType(), hasGuid.Id.ToString());
            }
            else
            {
                throw new ArgumentException($"Cannot deduce MongoDB document ID for entity of type: {entity.GetType().FullName}");
            }
        }

        protected string GetMongoId<T>(string id)
        {
            return GetMongoId(typeof(T), id);
        }

        protected string GetMongoId(Type entityType, string id)
        {
            return entityType.Name + "/" + id;
        }
    }
}
