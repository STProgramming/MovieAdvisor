using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAContracts.Contracts.Services
{
    public interface IReviewServices
    {
        Task PostNewReview(string userEmail, int movieId, string? descriptionVote, float vote, string? when);

        Task<List<ReviewDTO>?> SearchEngineReviews(string? userEmail, int? movieId);
    }
}
