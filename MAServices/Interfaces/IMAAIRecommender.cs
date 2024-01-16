using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAModels.Models;

namespace MAAI.Interfaces
{
    public interface IMAAIRecommender
    {
        Task<List<MovieSuggested>> NMoviesSuggestedByUser(User user);
    }
}
