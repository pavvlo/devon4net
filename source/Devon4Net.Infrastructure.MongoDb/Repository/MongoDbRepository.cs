using Devon4Net.Infrastructure.MongoDb.Common;
using Devon4Net.Infrastructure.MongoDb.Constants;
using Devon4Net.Infrastructure.MongoDb.MongoDb;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Devon4Net.Infrastructure.MongoDb.Repository
{
    public class MongoDbRepository<T, TContext> : IMongoDbRepository<T, TContext> where T : MongoEntity where TContext : MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<T> _collection;

        /// <summary>
        /// Initializes the repository for a type T which is the entity in the collection and
        /// a TContext where the entity will be stored
        /// </summary>
        /// <param name="serviceProvider">The .Net service provider</param>
        public MongoDbRepository(IServiceProvider serviceProvider)
        {
            _database = GetDatabaseFromServices(serviceProvider);
            _collection = _database.GetCollection<T>(typeof(T).Name);
        }

        private IMongoDatabase GetDatabaseFromServices(IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetService(typeof(TContext));
            if (service == null)
                throw new MongoDbException(string.Format(MongoDbConstants.ContextNotFoundMessage, typeof(TContext).FullName ?? ""));
            
            return (service as TContext).Database;
        }

        /// <summary>
        /// Inserts a single MongoEntity
        /// </summary>
        /// <param name="entity">The entity</param>
        public async Task Create(T entity)
        {
            await _collection.InsertOneAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// Inserts many documents
        /// </summary>
        /// <param name="entityCollection">The collection of entities</param>
        public async Task Create(IEnumerable<T> entityCollection)
        {
            await _collection.InsertManyAsync(entityCollection).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the entities matching the filter, if no filter is provided returns all the entities
        /// </summary>
        /// <param name="expression">LinQ expression to filter</param>
        /// <returns>The list of the entities</returns>
        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> expression = null)
        {
            
            var result = await _collection.FindAsync(expression ?? (_ => true)).ConfigureAwait(false);
            return await result.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the entity matching with the <paramref name="id"/> provided
        /// </summary>
        /// <param name="id">The id of the entity</param>
        /// <returns>The matching entity</returns>
        public async Task<T> Get(string id)
        {
            var result = await Get(e => e.Id == id).ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Finds and replaces an entity matching with the <c>entity.Id</c>
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The replaced entity</returns>
        public async Task<T> Replace(T entity)
        {
            var result = await _collection.FindOneAndReplaceAsync(e => e.Id == entity.Id, entity).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Finds and replaces an entity matching a filter
        /// </summary>
        /// <param name="expression">LinQ expression to filter</param>
        /// <param name="entity">The entity</param>
        /// <returns>The replaced entity</returns>
        public async Task<T> Replace(Expression<Func<T, bool>> expression, T entity)
        {
            var result = await _collection.FindOneAndReplaceAsync(expression, entity).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Finds and updates an entity with a filter
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <param name="update">The update</param>
        /// <returns>The updated entity</returns>
        public async Task<T> Update(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            var result = await _collection.FindOneAndUpdateAsync(filter, update).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Finds and deletes an entity
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>The deleted entity</returns>
        public async Task<T> Delete(T entity)
        {
            var result = await _collection.FindOneAndDeleteAsync(e => e.Id == entity.Id).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Deletes many documents
        /// </summary>
        /// <param name="entityCollection">The collection of entities</param>
        /// <returns>The deleted entities</returns>
        public async Task<IEnumerable<T>> Delete(IEnumerable<T> entityCollection)
        {
            var result = new List<T>();
            foreach (var entity in entityCollection)
                result.Add(await Delete(entity));
            return result;
        }

        /// <summary>
        /// Deletes the entity matching with the <paramref name="id"/> provided
        /// </summary>
        /// <param name="id">The id of the entity</param>
        /// <returns>The deleted entity</returns>
        public async Task<T> Delete(string id)
        {
            var result = await _collection.FindOneAndDeleteAsync(e => e.Id == id).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Deletes the entities matching with the filter
        /// </summary>
        /// <param name="expression">LinQ expression to filter</param>
        /// <returns>The number of deleted entities</returns>
        public async Task<long> Delete(Expression<Func<T, bool>> expression)
        {
            var result = await _collection.DeleteManyAsync(expression).ConfigureAwait(false);
            return result.DeletedCount;
        }

        /// <summary>
        /// Returns the number of entities in the collection. 
        /// Optionally, you can include a <paramref name="expression"/> to filter the collection
        /// You can also use the much faster method <see cref="MongoDbRepository{T, TContext}.EstimateCount"/> to estimate a count
        /// </summary>
        /// <param name="expression">LinQ expression to filter</param>
        /// <returns>The number of entities</returns>
        public async Task<long> Count(Expression<Func<T, bool>> expression = null)
        {
           return  await _collection.CountDocumentsAsync(expression ?? (_ => true)).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns an estimate of the number of entities in the collection
        /// </summary>
        /// <returns>The estimated number of entities</returns>
        public async Task<long> EstimateCount()
        {
            return await _collection.EstimatedDocumentCountAsync().ConfigureAwait(false);
        }
    }
}
