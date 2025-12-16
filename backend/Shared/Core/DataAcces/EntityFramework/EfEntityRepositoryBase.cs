using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.DataAcces.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public void Add(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Remove(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                return filter == null
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public void RemoveAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                var entities = context.Set<TEntity>().Where(filter).ToList();
                //var entities = filter == null
                //            ? context.Set<TEntity>().Where(x => false).ToList() 
                //            : context.Set<TEntity>().Where(filter).ToList();
                context.Set<TEntity>().RemoveRange(entities);
                context.SaveChanges();
            }
        }

        public int GetTotalCount(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext contex = new TContext())
            {
                return filter == null
                 ? contex.Set<TEntity>().Count()
                 : contex.Set<TEntity>().Count(filter);
            }
        }

        public TEntity GetAsNoTracking(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                return context.Set<TEntity>().AsNoTracking().SingleOrDefault(filter);
            }
        }
    }
}
