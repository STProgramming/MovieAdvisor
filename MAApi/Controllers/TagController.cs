using MAModels.EntityFrameworkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MAApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ApplicationDbContext _database;

        public TagController(ApplicationDbContext database)
        {
            _database = database;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovieTags()
        {
            return Ok(await _database.MoviesTags.OrderBy(x => x).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetMoviesFromMovieTag(int idMovieTag)
        {
            if (!_database.MoviesTags.Any(m => m.Id == idMovieTag)) return NotFound();
            var moviesIds = await _database.MoviesTags.Where(d => d.TagId == idMovieTag).ToListAsync();
            var movies = new List<Movie>();
            foreach (var id in moviesIds)
            {                
                if (!_database.Movies.Any(m => m.Id == id.MovieId)) return NotFound();
                Movie movie = (Movie)_database.Movies.Where(m => m.Id == id.MovieId);
                movies.Add(movie);
            }
            return Ok(movies);
        }
    }
}
