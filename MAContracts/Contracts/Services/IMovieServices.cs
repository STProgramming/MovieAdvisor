using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;


namespace MAContracts.Contracts.Services
{
    public interface IMovieServices
    {
        Task<List<MovieDTO>> SearchEngine(string Query);

        Task<int> CreateNewMovie(MovieDTO newMovie);
    }
}
