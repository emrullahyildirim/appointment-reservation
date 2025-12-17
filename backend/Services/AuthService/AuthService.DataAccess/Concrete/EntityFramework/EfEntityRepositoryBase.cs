using Core.DataAcces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthService.DataAccess.Concrete.EntityFramework
{
    /// <summary>
    /// DI-uyumlu Entity Repository Base sınıfı
    /// </summary>
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext
    {
        protected readonly TContext _context;

        public EfEntityRepositoryBase(TContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            var addedEntity = _context.Entry(entity);
            addedEntity.State = EntityState.Added;
            _context.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            var deletedEntity = _context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {

                IQueryable<TEntity> query = _context.Set<TEntity>();
                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                return query.SingleOrDefault();
            
        }

        public List<TEntity> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes)
        {

                IQueryable<TEntity> query = _context.Set<TEntity>();
                if (includes != null && includes.Length > 0)
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                return query.ToList();
            
        }

        public void Update(TEntity entity)
        {
            var updatedEntity = _context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public TEntity? GetAsNoTracking(Expression<Func<TEntity, bool>> filter)
        {
            return _context.Set<TEntity>().AsNoTracking().SingleOrDefault(filter);
        }

        public int GetTotalCount(Expression<Func<TEntity, bool>>? filter = null)
        {
            return filter == null
                ? _context.Set<TEntity>().Count()
                : _context.Set<TEntity>().Count(filter);
        }

        public void RemoveAll(Expression<Func<TEntity, bool>>? filter = null)
        {
            if (filter == null) return;
            
            var entities = _context.Set<TEntity>().Where(filter).ToList();
            _context.Set<TEntity>().RemoveRange(entities);
            _context.SaveChanges();
        }

        public async Task<List<TEntity>> GetAllAsyncAsNoTracking(Expression<Func<TEntity, bool>> filter = null)
        {

                IQueryable<TEntity> query =  _context.Set<TEntity>();

                if (filter != null)
                    query = query.Where(filter);

                return await query.AsNoTracking().ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {

                return await _context.Set<TEntity>()
                            .AsNoTracking()
                            .AnyAsync(filter);


        }
    }
}

