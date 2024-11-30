using Contracts;
using DataLayer;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class JwtBlacklistRepository: RepositoryBase<LoginModel>, IJwtBlacklistService
    {
        public JwtBlacklistRepository(RepositoryDBContext repositoryContext) : base(repositoryContext)
        {

        }

        public bool IsTokenBlacklisted(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var tokenId = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value;
            var userId = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            var user = this.RepositoryContext.LoginModels.Find(int.Parse(userId));

            if (user.LatestTokenId != tokenId)
            {
                return false;
            }

            // Proceed with normal token validation
            return true;
        }

       
    }
}
