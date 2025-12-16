using Core.Entities;
using System.Linq.Expressions;

namespace Core.DataAcces
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        int GetTotalCount(Expression<Func<T, bool>> filter = null);
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        T GetAsNoTracking(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveAll(Expression<Func<T, bool>> filter = null);

    }
}
