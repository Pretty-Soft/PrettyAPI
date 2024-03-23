using Contracts;
using Entities;
using Entities.Models;
using Entities.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OwnerRepository: RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(RepositoryDBContext repositoryContext):base(repositoryContext)
        {
              
        }
       
        public async void CreateOwner(Owner owner)=> await CreateAsync(owner);
        
        public async void UpdateOwner(Owner owner)=> await UpdateAsync(owner);
        
        public async void DeleteOwner(Owner owner)=> await DeleteAsync(owner);

        public Task<bool> GetOwnerWithAccountAsync(Guid id)
        {
           return this.RepositoryContext.Accounts.AnyAsync(m=>m.OwnerId==id);
        }
    }
}
