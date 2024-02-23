using MAModels.EntityFrameworkModels;

namespace MAModels.DTO
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; } = 0;

        public float Vote { get; set; } = 0;

        public string? DescriptionVote { get; set; } = string.Empty;

        public DateTime DateTimeVote { get; set; }

        public int UserId { get; set; } = 0;

        public int MovieId { get; set; } = 0;

        public ReviewDTO (Review review)
        {
            this.ReviewId = review.ReviewId;
            this.Vote = review.Vote;
            this.DescriptionVote = review.DescriptionVote;
            this.DateTimeVote = review.DateTimeVote;
            this.UserId = review.UserId;
            this.MovieId = review.MovieId;
        }
    }
}
