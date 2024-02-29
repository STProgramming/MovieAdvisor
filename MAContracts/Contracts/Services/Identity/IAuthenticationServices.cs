using Microsoft.AspNetCore.Authentication;

namespace MAContracts.Contracts.Services.Identity
{
    public interface IAuthenticationServices
    {
        Task<string> GoogleAuthentication(string returnUrl, string error);

        AuthenticationProperties LoginWithGoogle();
    }
}
