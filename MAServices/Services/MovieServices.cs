using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MAServices.MovieServices
{
    public class MovieServices : IMovieServices
    {
        private readonly ApplicationDbContext _database;

        private readonly IMovieTagServices _movieTagServices;

        public MovieServices(ApplicationDbContext database,
            IMovieTagServices movieTagServices)
        {
            _database = database;
            _movieTagServices = movieTagServices;
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
            return await _database.Movies.Where(m => string.Equals(movieTitle.ToLower().Trim(), m.MovieTitle.ToLower().Trim()) && movieYearProduction == m.MovieYearProduction && string.Equals(movieMaker.Trim().ToLower(), m.MovieMaker.Trim().ToLower())).ToListAsync();
        }

        public async Task CreateNewMovie(MovieDTO newMovie)
        {
            await _database.Movies.AddAsync(newMovie);
            await _database.SaveChangesAsync();
            newMovie.InsertMovieTagsId();
            if(newMovie.MovieTagsId != null)
            {
                List<MovieDescription> movieDesc = new List<MovieDescription>();
                foreach (var movieTag in newMovie.MovieTagsId)
                {
                    movieDesc.Add(await CreateMovieDescription(newMovie.MovieId, movieTag));
                }
                await _movieTagServices.SetListMovieTag(movieDesc);
            }
            _database.Movies.Update(newMovie);
            await _database.SaveChangesAsync();            
        }

        private async Task<MovieDescription> CreateMovieDescription(int movieId, int movieTagId)
        {
            var movieDesc = new MovieDescription();
            var movieTag = await _movieTagServices.GetMovieTag(movieTagId);
            var movie = await GetMovieData(movieId);
            if (movie != null && movieTag != null)
            {
                movieDesc.MovieTag = movieTag;
                movieDesc.MovieTagId = movieTagId;
                movieDesc.MovieId = movieId;
                movieDesc.Movie = movie;
                await _database.MoviesDescriptions.AddAsync(movieDesc);
                await _database.SaveChangesAsync();
            }
            return movieDesc;
        }

        public async Task AddNewMovieImage(IFormFileCollection ImageList, int movieId, string pathServer) 
        {
            List<MovieImage> imageList = new List<MovieImage>();
            var movie = await GetMovieData(movieId);
            if (movie == null) throw new ArgumentNullException();
            foreach (var image in ImageList)
            {
                MovieImage MovieImage = new MovieImage(image.FileName, pathServer, movieId, movie);
                await _database.MoviesImage.AddAsync(MovieImage);
                await _database.SaveChangesAsync();
                imageList.Add(MovieImage);
            }
            movie.MovieImages = imageList;
            _database.Movies.Update(movie);
            await _database.SaveChangesAsync();
        }

    }
}
