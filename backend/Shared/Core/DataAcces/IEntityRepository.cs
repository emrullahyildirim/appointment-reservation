using Core.Entities;
using System.Linq.Expressions;

namespace Core.DataAcces
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        int GetTotalCount(Expression<Func<T, bool>> filter = null);
        List<T> GetAll(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);
        T Get(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllAsyncAsNoTracking(Expression<Func<T, bool>> filter = null);
        T GetAsNoTracking(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveAll(Expression<Func<T, bool>> filter = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);


    }
}
