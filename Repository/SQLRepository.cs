using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class SQLRepository<TEntity> : ISQLRepositoryBase<TEntity> where TEntity : class
    {
        protected RepositoryDBContext RepositoryContext { get; set; }
      

        public SQLRepository(RepositoryDBContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
          
        }

        public async Task<IQueryable<TEntity>> ExecuteSqlQuery(Func<IQueryable<TEntity>, IQueryable<TEntity>> query)
        {
            return (IQueryable<TEntity>) await query(RepositoryContext.Set<TEntity>()).ToListAsync();
            
        }

    }
}
