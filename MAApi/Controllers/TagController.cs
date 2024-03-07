using MAContracts.Contracts.Services.Movie;
using MAModels.Enumerables.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MAApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagServices _tagServices;

        public TagController(ITagServices tagServices)
        {
            _tagServices = tagServices;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _tagServices.GetAllTags());
        }

        [Authorize(Roles = nameof(ERoleUser.AppAdmin))]
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await _tagServices.CreateAllTags();
            return StatusCode(201);
        }
    }
}
