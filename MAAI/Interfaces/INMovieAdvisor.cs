using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAModels.Models;

namespace MAAI.Interfaces
{
    public interface INMovieAdvisor
    {
        Task<List<MovieSuggested>> NMoviesSuggestedByUser(User user);
    }
}
