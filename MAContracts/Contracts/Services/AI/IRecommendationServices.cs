using MADTOs.DTOs.EntityFrameworkDTOs.AI;

namespace MAContracts.Contracts.Services.AI
{
    public interface IRecommendationServices
    {
        Task<List<RecommendationsDTO>> RecommendationsBasedOnReviews(string userId);

        Task<List<RecommendationsDTO>> RecommendationsBasedOnRequest(string userId, RequestsDTO requestUser);
    }
}
