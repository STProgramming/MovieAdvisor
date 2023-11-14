using MAModels.EntityFrameworkModels;
using MAModels.Enumerables;
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
        public async Task<IActionResult> GetAllTags()
        {
            return Ok(await _database.Tags.OrderBy(x => x).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetMoviesFromTag(int idTag)
        {
            if (!_database.MoviesTags.Any(m => m.MovieTagId == idTag)) return NotFound();
            var moviesIds = await _database.MoviesTags.Where(d => d.TagId == idTag).ToListAsync();
            var movies = new List<Movie>();
            foreach (var id in moviesIds)
            {                
                if (!_database.Movies.Any(m => m.MovieId == id.MovieId)) return NotFound();
                Movie movie = (Movie)_database.Movies.Where(m => m.MovieId == id.MovieId);
                movies.Add(movie);
            }
            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAllTags()
        {
            foreach (string name in Enum.GetNames(typeof(EMovieTags)))
            {
                Tag tag = new Tag
                {
                    TagName = name,
                };

                await _database.Tags.AddAsync(tag);
                await _database.SaveChangesAsync();

            }

            var allTags = await _database.Tags.ToListAsync();
            return StatusCode(201, allTags);
        }
    }
}
