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
        public async Task<IActionResult> GetAllTags()
        {
            return Ok(await _tagServices.GetAllTags());
        }

        [HttpGet]
        public async Task<IActionResult> GetMoviesFromTag(int idTag)
        {
            return Ok(await _tagServices.GetMoviesFromTag(idTag));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAllTags()
        {
            await _tagServices.CreateAllTags();
            return StatusCode(201);
        }
    }
}
