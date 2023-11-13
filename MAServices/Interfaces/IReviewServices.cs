using MAModels.EntityFrameworkModels;

namespace MAServices.Interfaces
{
    public interface IReviewServices
    {
        Task PostNewReview(User user, Movie movie, string? descriptionVote, short vote);

        Task<ICollection<Review>> GetReviews();

        Task<ICollection<Review>> GetReviewsOfUser(int userId);

        Task<ICollection<Review>> GetReviewsOfMovie(int movieId);

        Task<Review> GetYourRiviewOfMovie(int userId, int movieId);
    }
}
