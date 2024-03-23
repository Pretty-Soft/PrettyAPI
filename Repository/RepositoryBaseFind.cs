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
    public abstract class RepositoryBaseFind<T> :IRepositoryBaseFind<T> where T : class
    {
        protected RepositoryDBContext RepositoryContext { get; set; }
        public RepositoryBaseFind(RepositoryDBContext repositoryContext)    
        {
            RepositoryContext=repositoryContext;
        }

        public IQueryable<T> FindAll(Pagination pagination) => RepositoryContext.Set<T>().Skip(pagination.Skip).Take(pagination.Limit).AsQueryable();
        public IQueryable<T> FindByExpression(Expression<Func<T, bool>> expression)=> RepositoryContext.Set<T>().Where(expression).AsNoTracking();  
        public async Task<T> GetByIdAsync(int id) => await RepositoryContext.Set<T>().FindAsync(id);
        public async Task<long> GetCountAsync()=> await RepositoryContext.Set<T>().LongCountAsync();
    }
}
