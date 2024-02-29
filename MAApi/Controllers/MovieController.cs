using MAContracts.Contracts.Services;
using MADTOs.DTOs;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MoviesDTO NewMovie)
        {
            try
            {               
                return StatusCode(201, await _movieServices.CreateNewMovie(NewMovie));
            }
            catch (IOException)
            {
                return StatusCode(409);
            }
        }
    }
}
