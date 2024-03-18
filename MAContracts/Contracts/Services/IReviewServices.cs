using MADTOs.DTOs.EntityFrameworkDTOs;
using MADTOs.DTOs.ModelsDTOs;

namespace MAContracts.Contracts.Services
{
    public interface IReviewServices
    {
        Task PostNewReview(string userId, NewReviewDTO newReviewDto);

        Task<List<ReviewsDTO>?> SearchEngineReviews(string? userId, string? movieTitle);
    }
}
