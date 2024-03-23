using Entities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        IQueryable<T> FindByExpression(Expression<Func<T, bool>> expression);
        Task<IQueryable<T>> ExecuteSqlQuery(Func<IQueryable<T>, IQueryable<T>> query);
        Task<T> GetByIdAsync(Guid id);
        IQueryable<T> FindAll(Pagination pagination);
        Task<long> GetCountAsync();
    }
}
