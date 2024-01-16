﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAccountRepository: IRepositoryBase<Account>, IRepositoryBaseFind<Account>
    {
        Task<IEnumerable<Account>> AccountsByOwnerAsync(Guid ownerId);  
    }
}