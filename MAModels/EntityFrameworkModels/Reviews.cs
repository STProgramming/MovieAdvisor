using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MAModels.EntityFrameworkModels.Identity;

namespace MAModels.EntityFrameworkModels
{
    [Table("Reviews")]
    public class Reviews
    {
        [Key]
        [Required]
        public int ReviewId { get; set; }

        [Required, NotNull]
        public float Vote { get; set; }

        public string? DescriptionVote { get; set; }

        [Required]
        public DateTime DateTimeVote { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        public Users User { get; set; } = null!;

        [Required]
        public int MovieId { get; set; }

        public Movies Movie {  get; set; } = null!;

        public Reviews() { }
    }
}
