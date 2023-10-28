using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MAApi.Controllers
{
    [Route("api/[controller]/[action]")]
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

        [HttpGet]
        public async Task<IActionResult> GetUserData(string emailUser)
        {
            if (string.IsNullOrEmpty(emailUser) || !(_emailController.IsValid(emailUser))) return StatusCode(406);
            if (string.Equals(_configuration["EmailAdmin"], emailUser)) return Ok(await _userServices.GetAllUsers());
            else return Ok (await _userServices.GetUserData(emailUser));
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewUser(User newUser)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _userServices.GetUserData(newUser.EmailAddress);
            if (user == null) await _userServices.CreateNewUser(newUser);
            return StatusCode(201);
        }

        [HttpPut]
        public async Task<IActionResult> ModifyUserData([FromQuery] string userEmail, string? userEmailChange, string? userName)
        {
            if (!string.IsNullOrEmpty(userEmail) || !_emailController.IsValid(userEmail)) return StatusCode(401);
            var user = await _userServices.GetUserData(userEmail);
            if (user == null) return NotFound();
            if (!string.IsNullOrEmpty(userEmailChange) && _emailController.IsValid(userEmailChange))
            {
                var userCheck = await _userServices.GetUserData(userEmailChange);
                if (userCheck != null) return StatusCode(401);
                await _userServices.ModifyUserData(user, userEmailChange, userName);
                return Ok(new { message = "User is saved" });
            }
            else return Ok();
        }
    }
}
