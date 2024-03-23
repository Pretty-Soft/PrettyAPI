using Contracts;
using Entities;
using Entities.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryDBContext RepositoryContext { get; set; }
        public RepositoryBase(RepositoryDBContext repositoryContext)
        {
            RepositoryContext=repositoryContext;
        }

        public async Task CreateAsync(T entity)
        {
            await RepositoryContext.Set<T>().AddAsync(entity);
        }
        public Task UpdateAsync(T entity)
        {
            RepositoryContext.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
        public  Task DeleteAsync(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }
        public IQueryable<T> FindByExpression(Expression<Func<T, bool>> expression) => RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        public async Task<IQueryable<T>> ExecuteSqlQuery(Func<IQueryable<T>, IQueryable<T>> query)=> (IQueryable<T>)await query(RepositoryContext.Set<T>()).ToListAsync();
        public  async Task<T> GetByIdAsync(Guid id) => await RepositoryContext.Set<T>().FindAsync(id);
        public IQueryable<T> FindAll(Pagination pagination) => RepositoryContext.Set<T>().Skip(pagination.Skip).Take(pagination.Limit).AsQueryable();
        public async Task<long> GetCountAsync() => await RepositoryContext.Set<T>().LongCountAsync();
        
    }
}
