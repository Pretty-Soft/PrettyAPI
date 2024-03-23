using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ISQLRepositoryBase<TEntity> where TEntity : class
    {
        Task<IQueryable<TEntity>> ExecuteSqlQuery(Func<IQueryable<TEntity>, IQueryable<TEntity>> query);
        //Task<IQueryable<TEntity>> ExecuteRawQuery(string sqlQuery, params object[] parameters);
    }
}
