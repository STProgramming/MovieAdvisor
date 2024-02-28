using MAContracts.Contracts.Services.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public IActionResult GoogleRequest()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleResponse))
            };

            return Challenge(properties, "Google");
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            try
            {
                var authenticateResult = await HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
                return Ok(_authenticationServices.GoogleResponse(authenticateResult));
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }

        }
    }
}
