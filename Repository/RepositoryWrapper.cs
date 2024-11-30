using Contracts;
using DataLayer;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryWrapper(RepositoryDBContext repositoryContext) : IRepositoryWrapper, IDisposable
    {
        private RepositoryDBContext _repoContext = repositoryContext;
        private IOwnerRepository _owner;
        private IAccountRepository _account;
        private JwtBlacklistRepository _jwtBlackList;


        public IOwnerRepository Owner
        {
            get
            {
                if(_owner == null){_owner = new OwnerRepository(_repoContext);}
                return _owner;
            }
        }

        public IAccountRepository Account
        {
            get{
                if( _account == null) { _account = new AccountRepository(_repoContext); }
                return _account;
            }
        }
        public IJwtBlacklistService JWTBlackList
        {
            get
            {
                if (_jwtBlackList == null) { _jwtBlackList = new JwtBlacklistRepository(_repoContext); }
                return _jwtBlackList;
            }
        }

        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _repoContext.Dispose();
        }
    }
}
