using MAContracts.Contracts.Services.Identity.User;
using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.ModelsDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

namespace MAApi.Controllers.identity.User
{
    [Route("api/Identity/User/[controller]")]
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _userServices.GetUserFromEmail(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]NewUserDTO NewUserModel)
        {
            if (!ModelState.IsValid) return StatusCode((int)HttpStatusCode.NotAcceptable);
            try
            {
                await _userServices.CreateNewUser(NewUserModel);
                return StatusCode(201);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode((int)HttpStatusCode.Conflict);
            }
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
