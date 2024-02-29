using Duende.IdentityServer.Validation;
using MAContracts.Contracts.Services.Identity;
using MAModels.EntityFrameworkModels.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
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

        public async Task<string> GoogleAuthentication(string returnUrl, string error)
        {
            if (!string.IsNullOrEmpty(error)) throw new Exception();
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) throw new Exception();
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (!signInResult.Succeeded) throw new UnauthorizedAccessException();
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            var token = GenerateJwtToken(user);
            return token;
        }

        public AuthenticationProperties LoginWithGoogle()
        {
            return _signInManager.ConfigureExternalAuthenticationProperties("Google", "ExternalLoginCallback");
        }

        #endregion

        #region PRIVATE METHODS

        private string GenerateJwtToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        #endregion
    }
}
