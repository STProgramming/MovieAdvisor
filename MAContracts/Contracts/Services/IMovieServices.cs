using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;


namespace MAContracts.Contracts.Services
{
    public interface IMovieServices
    {
        Task<List<MoviesDTO>> SearchEngine(string Query);

        Task<int> CreateNewMovie(MoviesDTO newMovie);
    }
}
