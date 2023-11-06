using MAModels.EntityFrameworkModels;
using MAModels.Enumerables;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using MAModels.DTO;

namespace MAApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieTagController : ControllerBase
    {
        private readonly ApplicationDbContext _database;

        private readonly EmailAddressAttribute _emailController = new EmailAddressAttribute();

        private readonly IConfiguration _configuration;

        public MovieTagController(ApplicationDbContext database, IConfiguration configuration)
        {
            _database = database;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovieTag(string emailUser)
        {
            if (string.IsNullOrEmpty(emailUser) || !_emailController.IsValid(emailUser) || !string.Equals(_configuration["EmailAdmin"], emailUser)) return StatusCode(401);

            foreach (string name in Enum.GetNames(typeof(EMovieTags)))
            {
                if(!_database.MoviesTags.Any(m => string.Equals(name, m.MovieTags)))
                {
                    MovieTagDTO tag = new MovieTagDTO
                    {
                        MovieTags = name,
                        MovieDescriptions = null
                    };
                    
                    await _database.MoviesTags.AddAsync(tag);
                    await _database.SaveChangesAsync();             
                }
            }     
            
            var allTags = await _database.MoviesTags.ToListAsync();
            return StatusCode(201, allTags);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovieTags()
        {
            return Ok(await _database.MoviesTags.OrderBy(x => x).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetMoviesFromMovieTag(int idMovieTag)
        {
            if (!_database.MoviesTags.Any(m => m.MovieTagsId == idMovieTag)) return NotFound();
            var moviesIds = await _database.MoviesDescriptions.Where(d => d.MovieTagId == idMovieTag).ToListAsync();
            var movies = new List<Movie>();
            foreach (var id in moviesIds)
            {                
                if (!_database.Movies.Any(m => m.MovieId == id.MovieId)) return NotFound();
                Movie movie = (Movie)_database.Movies.Where(m => m.MovieId == id.MovieId);
                movies.Add(movie);
            }
            return Ok(movies);
        }
    }
}
