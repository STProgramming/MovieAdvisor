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

        private readonly IUserServices _userServices;

        private readonly EmailAddressAttribute _emailAddressAttribute = new EmailAddressAttribute(); 

        public MovieController(IMovieServices movieServices, 
            IConfiguration config,
            IWebHostEnvironment webHostEnvironment,
            IUploadFileServices uploadFileServices,
            IUserServices userServices
            )
        {
            _movieServices = movieServices;
            _config = config;   
            _webHostEnvironment = webHostEnvironment;
            _uploadFileServices = uploadFileServices;
            _userServices = userServices;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<MovieDTO>>> GetSuggestionMovieForUser(string emailUser)
        {
            if(string.IsNullOrEmpty(emailUser) || !_emailAddressAttribute.IsValid(emailUser)) return StatusCode(406);
            var user = await _userServices.GetUserFromEmail(emailUser);
            if (user == null) return NotFound();
            return Ok(await _movieServices.NSuggestedMoviesByUser(user));
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Movie>>> GetAllMovies()
        {
            return Ok(await _movieServices.GetAllMovies());
        }

        [HttpPost]
        public async Task<IActionResult> PostNewMovie([FromBody] MovieDTO newMovie)
        {
            if (!ModelState.IsValid) return StatusCode(406);
            var movie = await _movieServices.IsThisMovieAlreadyInDB(newMovie.MovieTitle, newMovie.MovieYearProduction, newMovie.MovieMaker);
            if (movie != null && movie.Count > 0) return StatusCode(406);
            await _movieServices.CreateNewMovie(newMovie);
            return StatusCode(201);
        }

        [HttpPost]
        public async Task<IActionResult> AddImagesToMovie([FromQuery]int MovieId, List<IFormFile> Files)
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
