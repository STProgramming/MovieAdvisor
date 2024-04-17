using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.ModelsDTOs;
using MAModels.EntityFrameworkModels;


namespace MAContracts.Contracts.Services
{
    public interface IMovieServices
    {
        Task<MoviesSearchResultsDTO> SearchEngine(string Query, short page, short elementsViewed);

        Task<int> CreateNewMovie(NewMovieDTO newMovie);
    }
}
