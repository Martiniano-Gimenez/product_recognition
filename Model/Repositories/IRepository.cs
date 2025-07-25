using Microsoft.EntityFrameworkCore.Query;
using Model.Domain;
using System.Linq.Expressions;

namespace Model.Repositories
{
    public interface IRepository<T, typeIdentifier>
        where T : class
        where typeIdentifier : struct
    {
        Task<PagingResult<T>> GetPagedListAsync(IQueryable<T> query, int offset, int pageSize);
        IQueryable<T> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool asNoTracking = true);
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool isAsNotTracking = false, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        Task CreateAsync(T entity);
        Task CreateListAsync(List<T> entities);
        void Remove(T entity);
    }
}
