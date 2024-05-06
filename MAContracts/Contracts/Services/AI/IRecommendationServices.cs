using MADTOs.DTOs.EntityFrameworkDTOs.AI;
using MADTOs.DTOs.ModelsDTOs.AI;

namespace MAContracts.Contracts.Services.AI
{
    public interface IRecommendationServices
    {
        Task<List<RecommendationsDTO>> RecommendationsBasedOnReviews(string userId);

        //Task<List<RecommendationsDTO>> RecommendationsBasedOnRequest(string userId, NewRequestDTO requestUser);
    }
}
