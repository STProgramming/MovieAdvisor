using MADTOs.DTOs.EntityFrameworkDTOs;
using MAModels.EntityFrameworkModels;

namespace MAContracts.Contracts.Services
{
    public interface IReviewServices
    {
        Task PostNewReview(string userEmail, int movieId, string? descriptionVote, float vote, string? when);

        Task<List<ReviewsDTO>?> SearchEngineReviews(string? userEmail, int? movieId);
    }
}
