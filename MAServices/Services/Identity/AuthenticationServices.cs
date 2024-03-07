using MAContracts.Contracts.Services.Identity;
using MADTOs.DTOs.ModelsDTOs;
using MAModels.EntityFrameworkModels.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MAServices.Services.identity
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IConfiguration _config;

        private readonly SignInManager<Users> _signInManager;

        private readonly UserManager<Users> _userManager;

        public AuthenticationServices(
            IConfiguration config,
            SignInManager<Users> signInManager,
            UserManager<Users> userManager) 
        {
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        #region PUBLIC SERVICES

        #region GOOGLE AUTHENTICATION

        public async Task<string> ResponseGoogleAuthentication(string returnUrl, string error)
        {
            if (!string.IsNullOrEmpty(error)) throw new Exception();
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) throw new Exception();
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (!signInResult.Succeeded) throw new UnauthorizedAccessException();
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            var token = await GenerateJwtToken(user);
            return token;
        }

        public AuthenticationProperties GoogleAuthentication(string redirectUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        }

        #endregion

        #region MOVIE ADVISOR AUTHENTICATION

        public async Task<string> MAAuthentication(LoginDTO login, HttpContext? ctx)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null) throw new Exception();
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password) == false)
            {
                await _userManager.AccessFailedAsync(user);
                throw new UnauthorizedAccessException();
            }
            if (ctx != null && _signInManager.IsSignedIn(ctx.User))
            {
                var emailClaimed = ctx.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (string.Equals(user.Email, emailClaimed)) throw new Exception();

                else await LogOutAuthentication(ctx);
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, login.StayConnected);

            if (!result.Succeeded || result.IsNotAllowed || result.IsLockedOut) throw new UnauthorizedAccessException();
            else return await GenerateJwtToken(user);
        }

        #endregion

        #region AUTHENTICATION SERVICES MANAGER

        public async Task LogOutAuthentication(HttpContext ctx)
        {
            var emailUser = ctx.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(emailUser);
            if (user == null) throw new UnauthorizedAccessException();
            ctx.SignOutAsync();
            await _signInManager.SignOutAsync();
        }

        #endregion

        #endregion

        #region PRIVATE METHODS

        private async Task<string> GenerateJwtToken(Users user)
        {
            var role = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, role.FirstOrDefault()),                    
                }),
                Audience = _config["JwtSettings:Audience"],
                Issuer = _config["JwtSettings:Issuer"],
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        #endregion
    }
}
