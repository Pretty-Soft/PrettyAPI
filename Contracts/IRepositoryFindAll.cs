using Entities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryFindAll<T> where T : class
    {
        IQueryable<T> FindAll(Pagination pagination);
        Task<long> GetCountAsync();
    }
}
