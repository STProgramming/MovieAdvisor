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

        private readonly IUploadFileServices _uploadFileServices;

        private readonly EmailAddressAttribute _emailController = new EmailAddressAttribute();

        public MovieController(IMovieServices movieServices, 
            IConfiguration config,
            IWebHostEnvironment webHostEnvironment,
            IUploadFileServices uploadFileServices
            )
        {
            _movieServices = movieServices;
            _config = config;   
            _webHostEnvironment = webHostEnvironment;
            _uploadFileServices = uploadFileServices;
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

        [HttpPost]
        public async Task<IActionResult> AddImagesToMovie([FromQuery]int MovieId, ICollection<IFormFile> Files)
        {            
            if(Files.Count == 0) return BadRequest();
            try
            {
                string contentRootPath = _webHostEnvironment.ContentRootPath;
                await _movieServices.AddNewMovieImage(Files, MovieId, _uploadFileServices.SaveImage(Files));
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
