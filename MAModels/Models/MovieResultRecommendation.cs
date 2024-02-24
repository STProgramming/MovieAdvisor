
namespace MAModels.Models
{
    public class MovieResultRecommendation
    {
        public int MovieId { get; set; }

        public string MovieTitle { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public float Score { get; set; }
    }
}
