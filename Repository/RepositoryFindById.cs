using Contracts;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryFindById<T>: IRepositoryFindById<T> where T : class
    {
        protected RepositoryDBContext RepositoryContext { get; set; }
        public RepositoryFindById(RepositoryDBContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }
        public async Task<T> GetByIdAsync(Guid id)=> await RepositoryContext.Set<T>().FindAsync(id);
        
    }
}
