using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using Microsoft.AspNetCore.Http;

namespace MAServices.Interfaces
{
    public interface IMovieServices
    {
        Task<List<MovieDTO>> NSuggestedMoviesByUser(User user);

        Task<List<Movie>> IsThisMovieAlreadyInDB(string movieTitle, short movieYearProduction, string movieMaker);

        Task<List<Movie>> GetAllMovies();

        Task CreateNewMovie(MovieDTO newMovie);

        Task<Movie?> GetMovieData(int movieId);

        Task AddNewMovieImage(List<IFormFile> ImageList, int movieId, List<string> serverPathsImage);
    }
}
