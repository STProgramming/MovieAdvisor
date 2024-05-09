using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAModels.EntityFrameworkModels.AI
{
    [Table("Recommendations")]
    public class Recommendations
    {
        [Key]
        public int RecommendationId { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public string MovieTitle { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public float AiScore { get; set; }

        [Required]
        public bool See { get; set; }

        [Required]
        public int RequestId { get; set; }

        public Requests Request { get; set; } = null!;

        public Recommendations() { }
    }
}
