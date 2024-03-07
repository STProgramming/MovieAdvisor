using MADTOs.DTOs.ModelsDTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace MAContracts.Contracts.Services.Identity
{
    public interface IAuthenticationServices
    {
        Task<string> ResponseGoogleAuthentication(string returnUrl, string error);

        AuthenticationProperties GoogleAuthentication(string redirectUrl);

        Task<string> MAAuthentication(LoginDTO login, HttpContext? ctx);

        Task LogOutAuthentication(HttpContext ctx);
    }
}
