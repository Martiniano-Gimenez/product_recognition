using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Model.Domain;
using Model.Repositories;
using System.Linq.Expressions;

namespace Persistance.Context
{
    public class EntityFrameworkRepository<T, typeIdentifier> : IRepository<T, typeIdentifier>
        where T : class
        where typeIdentifier : struct
    {
        protected readonly ShoppingCartContext _context;

        public EntityFrameworkRepository(ShoppingCartContext context)
        {
            _context = context;
        }

        public DbSet<T> DbSet
        {
            get { return _context.Set<T>(); }
        }

        public async Task<PagingResult<T>> GetPagedListAsync(IQueryable<T> query, int offset, int pageSize)
        {
            return new PagingResult<T>
            {
                Count = await query.CountAsync(),
                Results = await query.Skip(offset).Take(pageSize).ToListAsync()
            };
        }

        public IQueryable<T> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool asNoTracking = true)
        {
            IQueryable<T> result = _context.Set<T>();

            if (include != null)
                result = include(result);

            if (typeof(Activable).IsAssignableFrom(typeof(T)))
            {
                var parameter = Expression.Parameter(typeof(T));
                var property = Expression.Property(parameter, "IsActive");
                var propAsObject = Expression.Convert(property, typeof(bool));
                var expression = Expression.Lambda<Func<T, bool>>(propAsObject, parameter);
                result = result.Where(expression);
            }

            return asNoTracking ? result.AsNoTracking() : result;
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool isAsNotTracking = false, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> result = _context.Set<T>();

            if (include != null)
                result = include(result);

            if (typeof(Activable).IsAssignableFrom(typeof(T)))
            {
                var parameter = Expression.Parameter(typeof(T));
                var property = Expression.Property(parameter, "IsActive");
                var propAsObject = Expression.Convert(property, typeof(bool));
                var expressionIsActive = Expression.Lambda<Func<T, bool>>(propAsObject, parameter);
                result = result.Where(expressionIsActive);
            }

            return isAsNotTracking ? result.Where(expression).AsNoTracking() : result.Where(expression);
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task CreateListAsync(List<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
