
using Devon4Net.Infrastructure.MongoDb.Common;

namespace Devon4Net.Infrastructure.MongoDb.Repository
{
    public interface IRepository<T> where T : MongoEntity
    { 
        Task Create(T entity);
        Task<IEnumerable<T>> GetAll();
        Task Update(T entity);
        Task Replace(T entity);
        Task Delete(T entity);
    }
}
