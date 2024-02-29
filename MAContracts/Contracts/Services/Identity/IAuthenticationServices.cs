using Microsoft.AspNetCore.Authentication;

namespace MAContracts.Contracts.Services.Identity
{
    public interface IAuthenticationServices
    {
        string GoogleResponse(AuthenticateResult result);
    }
}
