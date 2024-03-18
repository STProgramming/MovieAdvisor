using MAContracts.Contracts.Services.Identity;
using MADTOs.DTOs.ModelsDTOs;
using Microsoft.AspNetCore.Authorization;
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

        //TODO CORS BLOCKED NEED TO GET MORE WORK

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Google(string ReturnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Authentication", new { ReturnUrl });
            return Challenge(_authenticationServices.GoogleAuthentication(redirectUrl), "Google");
        }
        
        private async Task<IActionResult> ExternalLoginCallback(string ReturnUrl, string RemoteError)
        {
            try
            {
                var token = await _authenticationServices.ResponseGoogleAuthentication(ReturnUrl, RemoteError);
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

        #region MOVIE ADVISOR AUTHENTICATION

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> MA([FromBody]LoginDTO login)
        {
            if (!ModelState.IsValid) return StatusCode((int)HttpStatusCode.NotAcceptable);
            try
            {
                var token = await _authenticationServices.MAAuthentication(login, HttpContext);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        #endregion

        #region AUTHENTICATION MANAGEMENT

        //TODO CHANGE PASSWORD RESET PASSWORD OTHER STUFF

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await _authenticationServices.LogOutAuthentication(HttpContext);
                return Ok();
            }
            catch
            {
                return Unauthorized();
            }
        }

        #endregion
    }
}
