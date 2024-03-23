using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.ModelsDTOs;

namespace MAContracts.Contracts.Services
{
    public interface IReviewServices
    {
        Task<List<ReviewsDTO>?> SearchEngineReviews(string? userId, string? movieTitle);

        Task PostNewReview(string userId, NewReviewDTO newReviewDto);

        Task EditReview(string userId, int reviewId, NewReviewDTO reviewModified);
    }
}
