using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MAApi.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet]
        public async Task<IActionResult> Get(string EmailUser)
        {
            if (string.IsNullOrEmpty(EmailUser) || !(_emailController.IsValid(EmailUser))) return StatusCode(406);
            if (string.Equals(_configuration["EmailAdmin"], EmailUser)) return Ok(await _userServices.GetAllUsers());
            else return Ok (await _userServices.GetUserFromEmail(EmailUser));
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserDTO NewUserModel)
        {
            if(!ModelState.IsValid) return StatusCode(406);
            var user = await _userServices.GetUserFromEmail(NewUserModel.EmailAddress);            
            if (user == null) await _userServices.CreateNewUser(NewUserModel);
            return StatusCode(201);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromQuery] string UserEmail, string? UserEmailChange, string? UserName)
        {
            if (!string.IsNullOrEmpty(UserEmail) || !_emailController.IsValid(UserEmail)) return StatusCode(406);
            var user = await _userServices.GetUserFromEmail(UserEmail);
            if (user == null) return NotFound();
            if (!string.IsNullOrEmpty(UserEmailChange) && _emailController.IsValid(UserEmailChange))
            {
                var userCheck = await _userServices.GetUserFromEmail(UserEmailChange);
                if (userCheck != null) return StatusCode(401);
                await _userServices.ModifyUserData(user, UserEmailChange, UserName);
                return Ok(new { message = "User is saved" });
            }
            else return Ok();
        }
    }
}
