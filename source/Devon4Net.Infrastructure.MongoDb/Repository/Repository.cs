using Devon4Net.Infrastructure.MongoDb.Common;
using Devon4Net.Infrastructure.MongoDb.MongoDb;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Devon4Net.Infrastructure.MongoDb.Repository
{
    public class Repository<T> : IRepository<T> where T : MongoEntity
    {
        private readonly IMongoDbContext _context;
        private readonly IMongoCollection<T> _collection;

        public Repository(IMongoDbContext mongoDbContext)
        {
            _context = mongoDbContext;
            _collection = mongoDbContext.Database.GetCollection<T>(typeof(T).Name);
        }

        public async Task Create(T entity)
        {
            await _collection.InsertOneAsync(entity).ConfigureAwait(false);
        }

        public async Task Create(IEnumerable<T> entityCollection)
        {
            foreach (var entity in entityCollection)
                await Create(entity);
        }

        public async Task<IEnumerable<T>> Get()
        {
            var result = await _collection.FindAsync(_ => true).ConfigureAwait(false);
            return await result.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> expression)
        {
            var result = await _collection.FindAsync(expression).ConfigureAwait(false);
            return await result.ToListAsync().ConfigureAwait(false);
        }

        // TODO check if not found
        public async Task<T> Get(string id)
        {
            var result = await Get(e => e.Id == id);
            //if (result.Count != 1) 
            return result.First();
        }

        public async Task<T> Replace(T entity)
        {
            var result = await _collection.FindOneAndReplaceAsync(e => e.Id == entity.Id, entity).ConfigureAwait(false);
            return result;
        }

        public async Task<T> Replace(Expression<Func<T, bool>> expression, T entity)
        {
            var result = await _collection.FindOneAndReplaceAsync(expression, entity).ConfigureAwait(false);
            return result;
        }

        // TODO continue working on this abobination
        public async Task Update(T entity)
        {
            var query = await _collection.FindAsync(e => e.Id == entity.Id).ConfigureAwait(false);
            var old = query.FirstOrDefault();
            await Replace(UpdateNonNullProperties(entity, old));
        }
        private static T UpdateNonNullProperties(T newEntity, T oldEntity)
        {
            foreach (var property in newEntity.GetType().GetProperties())
            {
                if (property.GetValue(newEntity) != null)
                    property.SetValue(oldEntity, property.GetValue(newEntity));
            }
            return oldEntity;
        }

        public async Task<T> Delete(T entity)
        {
            var result = await _collection.FindOneAndDeleteAsync(e => e.Id == entity.Id).ConfigureAwait(false);
            return result;
        }

        public async Task<IEnumerable<T>> Delete(IEnumerable<T> entityCollection)
        {
            var result = new List<T>();
            foreach (var entity in entityCollection)
                result.Add(await Delete(entity));
            return result;
        }

        public async Task<T> Delete(string id)
        {
            var result = await _collection.FindOneAndDeleteAsync(e => e.Id == id).ConfigureAwait(false);
            return result;
        }

        public async Task<long> Delete(Expression<Func<T, bool>> expression)
        {
            var result = await _collection.DeleteManyAsync(expression).ConfigureAwait(false);
            return result.DeletedCount;
        }

        public async Task<long> Count(Expression<Func<T, bool>>? expression = null)
        {
            if (expression == null) return await _collection.CountDocumentsAsync(_ => true).ConfigureAwait(false);
            return await _collection.CountDocumentsAsync(expression);
        }

        public async Task<long> EstimateCount()
        {
            return await _collection.EstimatedDocumentCountAsync().ConfigureAwait(false);
        }
    }
}
