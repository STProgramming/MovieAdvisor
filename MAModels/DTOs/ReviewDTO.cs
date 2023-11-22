using MAModels.EntityFrameworkModels;

namespace MAModels.DTO
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }

        public float Vote { get; set; }

        public string? DescriptionVote { get; set; } = string.Empty;

        public DateTime DateTimeVote { get; set; }

        public UserDTO User { get; set; } = null!;

        public MovieDTO Movie { get; set; } = null!;

        public ReviewDTO ConvertToReviewDTO(Review review)
        {
            this.ReviewId = review.ReviewId;
            this.Vote = review.Vote;
            this.DescriptionVote = review.DescriptionVote;
            this.DateTimeVote = review.DateTimeVote;
            UserDTO userDto = new UserDTO();
            MovieDTO movieDto = new MovieDTO();
            this.User = userDto.ConvertToUserDTO(review.User);
            this.Movie = movieDto.ConvertToMovieDTO(review.Movie);
            return this;
        }
    }
}
