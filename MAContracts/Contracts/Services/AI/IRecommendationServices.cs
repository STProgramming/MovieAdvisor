using MADTOs.DTOs.EntityFrameworkDTOs.AI;

namespace MAContracts.Contracts.Services.AI
{
    public interface IRecommendationServices
    {
        Task<List<RecommendationsDTO>> RecommendationsBasedOnReviews(string userEmail);

        Task<List<RecommendationsDTO>> RecommendationsBasedOnRequest(string userEmail, RequestsDTO requestUser);
    }
}
