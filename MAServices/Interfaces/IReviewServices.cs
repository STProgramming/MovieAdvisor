using MAModels.DTO;
using MAModels.EntityFrameworkModels;

namespace MAServices.Interfaces
{
    public interface IReviewServices
    {
        Task PostNewReview(string userEmail, int movieId, string? descriptionVote, float vote, string? when);

        Task<List<ReviewDTO>?> SearchEngineReviews(string? userEmail, int? movieId);
    }
}
