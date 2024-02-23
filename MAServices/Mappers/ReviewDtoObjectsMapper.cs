using MAContracts.Contracts.Mappers;
using MADTOs.DTOs;
using MAModels.EntityFrameworkModels;

namespace MAServices.Mappers
{
    public class ReviewDtoObjectsMapper : IReviewDtoObjectsMapper
    {
        public ReviewDtoObjectsMapper() { }

        public ReviewDTO ReviewMapperDto(Review review)
        {
            ReviewDTO reviewDto = new ReviewDTO
            {
                Vote = review.Vote,
                DescriptionVote = review.DescriptionVote,
                DateTimeVote = review.DateTimeVote,
                UserEmail = review.User.EmailAddress,
                MovieTitle = review.Movie.MovieTitle
            };
            
            return reviewDto;
        }
    }
}
