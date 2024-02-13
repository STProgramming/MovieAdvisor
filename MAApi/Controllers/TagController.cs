using MAServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MAApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagServices _tagServices;

        public TagController(ITagServices tagServices)
        {
            _tagServices = tagServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _tagServices.GetAllTags());
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await _tagServices.CreateAllTags();
            return StatusCode(201);
        }
    }
}
