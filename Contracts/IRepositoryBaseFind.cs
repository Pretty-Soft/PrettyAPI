using Entities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryBaseFind<T>
    {
        IQueryable<T> FindAll(Pagination pagination);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        long GetCount();

    }
    
}
