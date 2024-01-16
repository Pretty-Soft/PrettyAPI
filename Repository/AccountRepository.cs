using Contracts;
using Entities.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AccountRepository: RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryDBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Account>> AccountsByOwnerAsync(Guid ownerId)
        {
            Expression<Func<Account, bool>> expression = owner => owner.Id.Equals(ownerId);
            return await FindByCondition(expression)
                .ToArrayAsync();
        }
       
    }
}
