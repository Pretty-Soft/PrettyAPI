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
        public async Task<IEnumerable<Owner>> GetOwnersAsync(Pagination pagination)  
        {
            return await FindAll(pagination).ToListAsync();
        }
        public async Task<IList<Owner>> GetOwnerByIdAsync(Guid ownerId)
        {
            Expression<Func<Owner, bool>> expression = owner => owner.Id.Equals(ownerId);
            return await FindByExpression(expression).ToListAsync();
        }
        public async Task<IList<Owner>> GetOwnerWithAccountDetailsAsync(Guid ownerId)
        {
            Expression<Func<Owner, bool>> expression = owner => owner.Id.Equals(ownerId);
            return await FindByExpression(expression).Include(ac => ac.Accounts).ToListAsync();
        }
        public void CreateOwner(Owner owner)=> CreateAsync(owner);
        
        public void UpdateOwner(Owner owner)=> UpdateAsync(owner);
        
        public void DeleteOwner(Owner owner)=> DeleteAsync(owner);
        
    }
}
