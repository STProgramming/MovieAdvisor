using MAModels.EntityFrameworkModels;
using MAModels.Enumerables;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> CreateAllMovieTag(string emailUser)
        {
            if (string.IsNullOrEmpty(emailUser) || !_emailController.IsValid(emailUser) || !string.Equals(_configuration["EmailAdmin"], emailUser)) return StatusCode(401);

            foreach (string name in Enum.GetNames(typeof(EMovieTags)))
            {
                MovieTag tag = new MovieTag
                {
                    MovieTags = name,
                    MovieTagsDescriptionsList = null
                };
                    
                await _database.MoviesTags.AddAsync(tag);
                await _database.SaveChangesAsync();
             
            }     
            
            var allTags = await _database.MoviesTags.ToListAsync();
            return StatusCode(201, allTags);
        }

        [HttpGet]
        public async Task<IActionResult> GetMovieTags()
        {
            return Ok(await _database.MoviesTags.ToListAsync());
        }
    }
}
