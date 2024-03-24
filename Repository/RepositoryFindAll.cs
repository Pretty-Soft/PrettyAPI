using Contracts;
using DataLayer;
using Entities;
using Entities.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryFindAll<T> : IRepositoryFindAll<T> where T : class
    {
        protected RepositoryDBContext RepositoryContext { get; set; }
        public RepositoryFindAll(RepositoryDBContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }   
        public IQueryable<T> FindAll(Pagination pagination) => RepositoryContext.Set<T>().Skip(pagination.Skip).Take(pagination.Limit).AsQueryable();
        public async Task<long> GetCountAsync() => await RepositoryContext.Set<T>().LongCountAsync();
    }
}
