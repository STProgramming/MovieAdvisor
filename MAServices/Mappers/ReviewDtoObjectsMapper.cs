using MAContracts.Contracts.Mappers;
using MADTOs.DTOs.EntityFrameworkDTOs;
using MAModels.EntityFrameworkModels;

namespace MAServices.Mappers
{
    public class ReviewDtoObjectsMapper : IReviewDtoObjectsMapper
    {
        public ReviewDtoObjectsMapper() { }

        public ReviewsDTO ReviewMappingDto(Reviews review)
        {
            ReviewsDTO reviewDto = new ReviewsDTO
            {
                Vote = review.Vote,
                DescriptionVote = review.DescriptionVote,
                DateTimeVote = review.DateTimeVote,
                UserEmail = review.User.Email,
                MovieTitle = review.Movie.MovieTitle
            };
            
            return reviewDto;
        }
    }
}
