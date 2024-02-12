using MAModels.DTO;
using MAServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MAApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieServices _movieServices;

        private readonly IFileServices _uploadFileServices;

        public MovieController(IMovieServices movieServices, 
            IFileServices uploadFileServices)
        {
            _movieServices = movieServices;
            _uploadFileServices = uploadFileServices;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<MovieDTO>>> Get(string Search = "")
        {
            return Ok(await _movieServices.SearchEngine(Search));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MovieDTO newMovie)
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

        [HttpPost]
        public async Task<ActionResult> PostMovieImage([FromQuery]int MovieId, List<IFormFile> Files)
        {            
            try
            {
                await _movieServices.AddNewMovieImage(Files, MovieId, _uploadFileServices.ConvertToByteArray(Files));
                return StatusCode(201);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }
    }
}
