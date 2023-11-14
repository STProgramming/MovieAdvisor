using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
 
namespace MAServices.MovieServices
{
    public class MovieServices : IMovieServices
    {
        private readonly ApplicationDbContext _database;

        private readonly ITagServices _tagServices;

        public MovieServices(ApplicationDbContext database,
            ITagServices tagServices)
        {
            _database = database;
            _tagServices = tagServices;
        }

        public async Task<ICollection<Movie>> GetAllMoviesFilteredByUser(User user)
        {
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
            Movie newMovieObj = new Movie
            {
                MovieTitle = newMovie.MovieTitle,
                MovieYearProduction = newMovie.MovieYearProduction,
                MovieMaker = newMovie.MovieMaker,
                MovieDescription = newMovie.MovieDescription,
            };
            await _database.Movies.AddAsync(newMovieObj);
            await _database.SaveChangesAsync();
            List<Tag> tagsInserted = new List<Tag>();
            List<MovieTag> moviesTags = new List<MovieTag>(); 
            if (newMovie.TagsId != null && newMovie.TagsId.Count > 0)
            {
                foreach(int tag in newMovie.TagsId)
                {
                    var tagObj = await _tagServices.GetTag(tag);
                    if (tagObj == null) throw new ArgumentNullException();
                    tagsInserted.Add(tagObj);
                    MovieTag? movieTagObj = new MovieTag();
                    movieTagObj = await _tagServices.AssociateTagToMovie(newMovieObj.MovieId, newMovieObj, tagObj.TagId);
                    if (movieTagObj == null) throw new ArgumentNullException();
                    moviesTags.Add(movieTagObj);
                }
            }
            newMovieObj.TagsList = tagsInserted;
            _database.Movies.Update(newMovieObj);
            await _tagServices.AssociateMovieToTag(moviesTags);
        }

        public async Task AddNewMovieImage(ICollection<IFormFile> ImageList, int movieId, List<string> serverPathsImage) 
        {
            List<Image> imageList = new List<Image>();
            var movie = await GetMovieData(movieId);
            if (movie == null) throw new ArgumentNullException();
            int counter = 0;
            foreach (var image in ImageList)
            {
                ImageDTO MovieImage = new ImageDTO(image.FileName, serverPathsImage[counter], movieId, movie);
                await _database.Images.AddAsync(MovieImage);
                await _database.SaveChangesAsync();
                imageList.Add(MovieImage);
                counter++;
            }
            movie.ImagesList = imageList;
            _database.Movies.Update(movie);
            await _database.SaveChangesAsync();
        }

    }
}
