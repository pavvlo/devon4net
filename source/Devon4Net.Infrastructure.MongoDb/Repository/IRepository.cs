using Devon4Net.Infrastructure.MongoDb.Common;
using MongoDB.Driver;
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
        Task<T> Update(FilterDefinition<T> filter, UpdateDefinition<T> update);

        Task<T> Replace(T entity);
        Task<T> Replace(Expression<Func<T, bool>> expression, T entity);

        Task<T> Delete(T entity);
        Task<IEnumerable<T>> Delete(IEnumerable<T> entityCollection);
        Task<long> Delete(Expression<Func<T, bool>> expression);
        Task<T> Delete(string id);

        Task<long> Count(Expression<Func<T, bool>>? expression = null);
        Task<long> EstimateCount();

    }
}
