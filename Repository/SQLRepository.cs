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
using DataLayer;

namespace Repository
{
    public class SQLRepository:ISQLRepositoryBase
    {
        protected RepositoryDBContext RepositoryContext { get; set; }
      

        public SQLRepository(RepositoryDBContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
          
        }

        //public async Task<IDictionary<string,object>> ExecuteSqlQuery(string sqlQuery, params object[] parameters)
        //{

        //    // Execute the SQL query and map results to MyEntity objects
        //    var myEntities = RepositoryContext.Database.FromSqlRaw(sqlQuery).FirstOrDefaultAsync();

        //    return myEntities.Result;
        //}

    }
}
