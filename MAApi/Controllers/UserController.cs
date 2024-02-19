using MAModels.DTO;
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

        //TODO refactoring del codice controllo come gli attuali controllers

        [HttpGet]
        public async Task<IActionResult> Get(string emailUser)
        {
            if (string.IsNullOrEmpty(emailUser) || !(_emailController.IsValid(emailUser))) return StatusCode(406);
            if (string.Equals(_configuration["EmailAdmin"], emailUser)) return Ok(await _userServices.GetAllUsers());
            else return Ok (await _userServices.GetUserFromEmail(emailUser));
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserDTO newUserModel)
        {
            if(!ModelState.IsValid) return StatusCode(406);
            var user = await _userServices.GetUserFromEmail(newUserModel.EmailAddress);            
            if (user == null) await _userServices.CreateNewUser(newUserModel);
            return StatusCode(201);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromQuery] string userEmail, string? userEmailChange, string? userName)
        {
            if (!string.IsNullOrEmpty(userEmail) || !_emailController.IsValid(userEmail)) return StatusCode(406);
            var user = await _userServices.GetUserFromEmail(userEmail);
            if (user == null) return NotFound();
            if (!string.IsNullOrEmpty(userEmailChange) && _emailController.IsValid(userEmailChange))
            {
                var userCheck = await _userServices.GetUserFromEmail(userEmailChange);
                if (userCheck != null) return StatusCode(401);
                await _userServices.ModifyUserData(user, userEmailChange, userName);
                return Ok(new { message = "User is saved" });
            }
            else return Ok();
        }
    }
}
