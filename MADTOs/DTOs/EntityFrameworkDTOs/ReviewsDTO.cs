namespace MADTOs.DTOs.EntityFrameworkDTOs
{
    public class ReviewsDTO
    {
        public int ReviewId { get; set; } = 0;

        public float Vote { get; set; } = 0;

        public string? DescriptionVote { get; set; } = string.Empty;

        public DateTime DateTimeVote { get; set; }

        public string UserEmail { get; set; } = string.Empty;

        public string MovieTitle { get; set; } = string.Empty;
    }
}
