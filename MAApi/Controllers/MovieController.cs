using MAModels.DTO;
using MAServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MAApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieServices _movieServices;

        public MovieController(IMovieServices movieServices)
        {
            _movieServices = movieServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string Search = "")
        {
            return Ok(await _movieServices.SearchEngine(Search));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MovieDTO newMovie)
        {
            try
            {
                await _movieServices.CreateNewMovie(newMovie);
                return StatusCode(201);
            }
            catch (IOException)
            {
                return StatusCode(409);
            }
        }
    }
}
