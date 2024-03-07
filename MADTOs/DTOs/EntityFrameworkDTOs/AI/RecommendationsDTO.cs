namespace MADTOs.DTOs.EntityFrameworkDTOs.AI
{
    public class RecommendationsDTO
    {
        public int RecommendationId { get; set; }

        public int MovieId { get; set; }

        public string MovieTitle { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public float AiScore { get; set; }

        public bool See { get; set; }

        public RequestsDTO Request { get; set; }
    }
}
