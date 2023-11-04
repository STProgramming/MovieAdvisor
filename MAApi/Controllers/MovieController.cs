using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MAApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IMovieServices _movieServices;

        private readonly IConfiguration _config;

        private readonly EmailAddressAttribute _emailController = new EmailAddressAttribute();

        public MovieController(IMovieServices movieServices, 
            IConfiguration config,
            IWebHostEnvironment webHostEnvironment)
        {
            _movieServices = movieServices;
            _config = config;   
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Movie>>> GetAllMoviesOfUser(string emailUser)
        {
            return Ok(await _movieServices.GetAllMoviesFilteredByUser(emailUser));
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Movie>>> GetAllMovies()
        {
            return Ok(await _movieServices.GetAllMovies());
        }

        [HttpPost]
        public async Task<IActionResult> PostNewMovie(string emailUser, [FromBody] MovieDTO newMovie)
        {
            if (string.IsNullOrEmpty(emailUser) || !_emailController.IsValid(emailUser) || string.Equals(emailUser, _config["EmailAddress"]) || !ModelState.IsValid) return StatusCode(400);
            var movie = await _movieServices.IsThisMovieAlreadyInDB(newMovie.MovieTitle, newMovie.MovieYearProduction, newMovie.MovieMaker);
            if (movie != null && movie.Count > 0) return BadRequest();
            await _movieServices.CreateNewMovie(newMovie);
            return StatusCode(201);
        }
    }
}
