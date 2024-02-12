using MAModels.DTO;
using MAModels.EntityFrameworkModels;
using MAModels.Models;

namespace MAAI.Interfaces
{
    public interface IRecommendationServices
    {
        Task<List<MovieResultRecommendation>> MoviesSuggestedByUser(string userEmail);
    }
}
