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
        public IQueryable<T> FindAll(Pagination pagination) =>  RepositoryContext.Set<T>().Skip(pagination.Skip).Take(pagination.Limit).AsQueryable();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
        public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
        public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
        public long GetCount()=> RepositoryContext.Set<T>().Count();  
        
    }
}
