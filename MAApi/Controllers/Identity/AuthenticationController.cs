using MAContracts.Contracts.Services.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MAApi.Controllers.Identity
{
    [Route("api/Identity/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;        

        public AuthenticationController(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }

        #region GOOGLE AUTHENTICATION

        #region PUBLIC

        [HttpGet]
        public IActionResult Google()
        {
            return Challenge(_authenticationServices.LoginWithGoogle(), "Google");
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string ReturnUrl, string RemoteError)
        {
            try
            {
                var token = await _authenticationServices.GoogleAuthentication(ReturnUrl, RemoteError);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable);
            }
        }

        #endregion

        #endregion
    }
}
