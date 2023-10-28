using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MAServices.MovieServices
{
    public class MovieServices : IMovieServices
    {
        private ApplicationDbContext _database;

        public MovieServices(ApplicationDbContext database)
        {
            _database = database;
        }

        public async Task<ICollection<Movie>> GetAllMoviesFilteredByUser(string? EmailUser)
        {
            var user = EmailUser != null && _database.Users.Any(x => string.Equals(x.EmailAddress, EmailUser)) ? await _database.Users.Where(x => string.Equals(x.EmailAddress, EmailUser)).FirstOrDefaultAsync() : null;
            //TODO return MovieAdvisorAI 
            var movies = new List<Movie>();
            return movies;
        }

        public async Task<ICollection<Movie>> GetAllMovies()
        {
            return await _database.Movies.ToListAsync();
        }

        public async Task<Movie?> GetMovieData(int movieId)
        {
            return await _database.Movies.Where(m => m.MovieId == movieId).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Movie>> IsThisMovieAlreadyInDB(string movieTitle, short movieYearProduction, string movieMaker)
        {
            return await _database.Movies.Where(m => string.Equals(movieMaker.ToLower().Trim(), m.MovieTitle.ToLower().Trim()) && movieYearProduction == m.MovieYearProduction && string.Equals(movieMaker.Trim().ToLower(), m.MovieMaker.Trim().ToLower())).ToListAsync();
        }

        public async Task CreateNewMovie(MovieDTO newMovie)
        {
            await _database.Movies.AddAsync(newMovie);
            await _database.SaveChangesAsync();
            newMovie.InsertMovieTagsId();
            newMovie.InsertPhoto();
            _database.Movies.Update(newMovie);
            await _database.SaveChangesAsync();
        }
    }
}
