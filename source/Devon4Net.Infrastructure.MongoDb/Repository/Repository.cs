using Devon4Net.Infrastructure.MongoDb.Common;
using Devon4Net.Infrastructure.MongoDb.MongoDb;
using MongoDB.Bson;
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

        public Task Create(IEnumerable<T> entityCollection)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> Get()
        {
            var result = await _collection.FindAsync(_ => true).ConfigureAwait(false);
            return await result.ToListAsync();
        }
        public Task<IEnumerable<T>> Get(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<T> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task Replace(T entity)
        { 
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity).ConfigureAwait(false);
        }

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

        public Task Delete(IEnumerable<T> entityCollection)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> Delete(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
