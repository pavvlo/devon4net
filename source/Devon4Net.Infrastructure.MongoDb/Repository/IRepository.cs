using Devon4Net.Infrastructure.MongoDb.Common;
using System.Linq.Expressions;

namespace Devon4Net.Infrastructure.MongoDb.Repository
{
    public interface IRepository<T> where T : MongoEntity
    { 
        Task Create(T entity);
        Task Create(IEnumerable<T> entityCollection);
        Task<IEnumerable<T>> Get();
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> expression);
        Task<T> Get(string id);
        Task Update(T entity);
        Task Replace(T entity);
        Task <T> Delete(T entity);
        Task Delete(IEnumerable<T> entityCollection);
        Task<IEnumerable<T>> Delete(Expression<Func<T, bool>> expression);
        Task Delete(string id);
    }
}
