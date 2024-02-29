using MAApi.Helpers.Identity;
using MAContracts.Contracts.Services.Identity.User;
using MADTOs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MAApi.Controllers.identity.user
{
    [Route("api/Identity/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        private readonly EmailAddressAttribute _emailController = new EmailAddressAttribute();

        private readonly IConfiguration _configuration;

        public UserController(IUserServices userServices, IConfiguration configuration)
        {
            _userServices = userServices;
            _configuration = configuration;
        }

        //TODO refactoring del codice controllo come gli attuali controllers
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var claims = IdentityHelpers.GetClaimsFromJwt(HttpContext.User);
                return Ok(await _userServices.GetUserFromEmail(claims.Where(c => c.Equals("emailUser")).ToString()));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(UsersDTO NewUserModel)
        {
            if (!ModelState.IsValid) return StatusCode(406);
            var user = await _userServices.GetUserFromEmail(NewUserModel.EmailAddress);
            if (user == null) await _userServices.CreateNewUser(NewUserModel);
            return StatusCode(201);
        }

        //[HttpPut]
        //public async Task<IActionResult> Put([FromQuery] string UserEmail, string? UserEmailChange, string? UserName)
        //{
        //    if (!string.IsNullOrEmpty(UserEmail) || !_emailController.IsValid(UserEmail)) return StatusCode(406);
        //    var user = await _userServices.GetUserFromEmail(UserEmail);
        //    if (user == null) return NotFound();
        //    if (!string.IsNullOrEmpty(UserEmailChange) && _emailController.IsValid(UserEmailChange))
        //    {
        //        var userCheck = await _userServices.GetUserFromEmail(UserEmailChange);
        //        if (userCheck != null) return StatusCode(401);
        //        try
        //        {
        //            await _userServices.ModifyUserData(user, UserEmailChange, UserName);
        //        }
        //        catch (NullReferenceException)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(new { message = "User is saved" });
        //    }
        //    else return Ok();
        //}
    }
}
