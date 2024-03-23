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

        public async void CreateAccount(Account account) => await CreateAsync(account);

        public async void UpdateOwner(Account account) => await UpdateAsync(account);

        public async void DeleteOwner(Account account) => await DeleteAsync(account);

    }
}
