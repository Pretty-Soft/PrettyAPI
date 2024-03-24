using Contracts;
using DataLayer;
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
    public abstract class RepositoryFindByExpression<T> : IRepositoryFindByExpression<T> where T : class
    {
        protected RepositoryDBContext RepositoryContext { get; set; }
        public RepositoryFindByExpression(RepositoryDBContext repositoryContext)     
        {
            RepositoryContext=repositoryContext;
        }

        public IQueryable<T> FindByExpression(Expression<Func<T, bool>> expression)=> RepositoryContext.Set<T>().Where(expression).AsNoTracking();  
    }
}
