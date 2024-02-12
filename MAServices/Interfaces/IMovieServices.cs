using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAModels.Models;
using Microsoft.AspNetCore.Http;

namespace MAServices.Interfaces
{
    public interface IMovieServices
    {
        Task<List<MovieDTO>> SearchEngine(string Query);

        Task<Movie> GetMovieDataById(int movieId);

        Task CreateNewMovie(MovieDTO newMovie);

        Task AddNewMovieImage(List<IFormFile> ImageList, int movieId, List<byte[]> imagesList);
    }
}
