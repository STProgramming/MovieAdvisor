using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using Microsoft.AspNetCore.Http;

namespace MAServices.Interfaces
{
    public interface IMovieServices
    {
        Task<ICollection<Movie>> GetAllMoviesFilteredByUser(User user);

        Task<ICollection<Movie>> IsThisMovieAlreadyInDB(string movieTitle, short movieYearProduction, string movieMaker);

        Task<ICollection<Movie>> GetAllMovies();

        Task CreateNewMovie(MovieDTO newMovie);

        Task<Movie?> GetMovieData(int movieId);

        Task AddNewMovieImage(ICollection<IFormFile> ImageList, int movieId, List<string> serverPathsImage);
    }
}
