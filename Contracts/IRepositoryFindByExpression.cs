using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryFindByExpression<T> where T : class
    {
        IQueryable<T> FindByExpression(Expression<Func<T, bool>> expression);
    }
}
