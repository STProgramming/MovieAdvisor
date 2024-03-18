using MAContracts.Contracts.Services.AI;
using MADTOs.DTOs.EntityFrameworkDTOs.AI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MAApi.Controllers.AI
{
    [Authorize]
    [Route("api/AI/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionServices _sessionServices;

        public SessionController(
            ISessionServices sessionServices)
        {
            _sessionServices = sessionServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _sessionServices.GetSessionsByUser(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        } 
    }
}
