using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.ModelsDTOs;
using MAModels.EntityFrameworkModels;


namespace MAContracts.Contracts.Services
{
    public interface IMovieServices
    {
        Task<List<MoviesDTO>> SearchEngine(string Query);

        Task<int> CreateNewMovie(NewMovieDTO newMovie);
    }
}
