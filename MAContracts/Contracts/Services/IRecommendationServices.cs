using MAModels.EntityFrameworkModels;
using MAModels.Models;

namespace MAContracts.Contracts.Services
{
    public interface IRecommendationServices
    {
        Task<List<MovieResultRecommendation>> MoviesSuggestedByUser(string userEmail);
    }
}
