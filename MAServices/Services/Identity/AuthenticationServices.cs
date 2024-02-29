using MAContracts.Contracts.Services.Identity;
using Microsoft.AspNetCore.Authentication;
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

        public AuthenticationServices(IConfiguration config) 
        {
            _config = config;
        }

        #region PUBLIC SERVICES

        public string GoogleResponse(AuthenticateResult result)
        {
            if (!result.Succeeded) throw new Exception();
            // Handle successful authentication (e.g., create JWT token)
            var token = GenerateJwtToken(result.Principal.Claims.ToList());
            return token;
        }

        #endregion

        #region PRIVATE METHODS

        private string GenerateJwtToken(IList<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JwtSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion
    }
}
