using Contracts;
using DataLayer;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrettyAPI.Models;

namespace PrettyAPI.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly RepositoryDBContext _DbContext;
        private readonly ITokenService _tokenService;
        public TokenController(RepositoryDBContext dbContext, ITokenService tokenService)
        {
            this._DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }
        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody]TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");

            string? accessToken = tokenApiModel?.AccessToken;
            string? refreshToken = tokenApiModel?.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            var username = principal.Identity?.Name; //this is mapped to the Name claim by default
            var user = _DbContext.LoginModels.SingleOrDefault(u => u.UserName == username);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            _DbContext.SaveChanges();

            return Ok(new AuthenticatedResponse()
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            string? username = User.Identity?.Name;
            var user = _DbContext.LoginModels.SingleOrDefault(u => u.UserName == username);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            _DbContext.SaveChanges();
            return NoContent();
        }
    }
}
