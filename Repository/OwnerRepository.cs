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
    public class OwnerRepository: RepositoryBase<Owner>,IOwnerRepository
    {
        public OwnerRepository(RepositoryDBContext repositoryContext):base(repositoryContext)
        {
                
        }

        public async Task<IEnumerable<Owner>> GetOwnersAsync(Pagination pagination)  
        {
            return await FindAll(pagination).ToListAsync();
        }
        public async Task<Owner> GetOwnerByIdAsync(Guid ownerId)
        {
            Expression<Func<Owner, bool>> expression = owner => owner.Id.Equals(ownerId);
            return await FindByCondition(expression)
                .FirstOrDefaultAsync();
        }
        public async Task<Owner> GetOwnerWithDetailsAsync(Guid ownerId)
        {
            Expression<Func<Owner, bool>> expression = owner => owner.Id.Equals(ownerId);
            return await FindByCondition(expression).Include(ac => ac.Accounts)
                .FirstOrDefaultAsync();
        }
        public void CreateOwner(Owner owner)
        {
            Create(owner);
        }
        public void UpdateOwner(Owner owner)
        {
            Update(owner);
        }
        public void DeleteOwner(Owner owner)
        {
            Delete(owner);
        }
    }
}
