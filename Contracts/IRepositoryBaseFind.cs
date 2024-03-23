using Entities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryBaseFind<T> where T : class
    {
        IQueryable<T> FindAll(Pagination pagination);
        IQueryable<T> FindByExpression(Expression<Func<T, bool>> expression);   
        Task<T> GetByIdAsync(int id);
        Task<long> GetCountAsync(); 
    }
    
}
