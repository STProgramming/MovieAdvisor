using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAModels.Models;
using Microsoft.AspNetCore.Http;

namespace MAServices.Interfaces
{
    public interface IMovieServices
    {
        Task<List<MovieSuggested>> NSuggestedMoviesByUser(User user);

        Task<List<Movie>> IsThisMovieAlreadyInDB(string movieTitle, short movieYearProduction, string movieMaker);

        Task<List<Movie>> GetAllMovies();

        Task CreateNewMovie(MovieDTO newMovie);

        Task<Movie?> GetMovieData(int movieId);

        Task AddNewMovieImage(List<IFormFile> ImageList, int movieId, List<byte[]> imagesList);
        }
}
