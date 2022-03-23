using Devon4Net.Infrastructure.MongoDb.Common;
using Devon4Net.Infrastructure.MongoDb.MongoDb;
using MongoDB.Driver;

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

        public async Task<IEnumerable<T>> GetAll()
        {
            var result = await _collection.FindAsync(_ => true).ConfigureAwait(false);
            return await result.ToListAsync();
        }

        public async Task Replace(T entity)
        { 
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity).ConfigureAwait(false);
        }

        public Task Update(T entity)
        {

            throw new NotImplementedException();
        }

        public async Task Delete(T entity)
        {
            await _collection.DeleteOneAsync(e => e.Id == entity.Id).ConfigureAwait(false);
        }


    }
}
