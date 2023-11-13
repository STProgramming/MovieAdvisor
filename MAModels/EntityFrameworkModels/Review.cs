using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MAModels.EntityFrameworkModels
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required, NotNull]
        public short Vote { get; set; }

        public string? DescriptionVote { get; set; }

        [Required]
        public DateTime DateTimeVote { get { return this.DateTimeVote; } set { this.DateTimeVote = DateTime.Now; } }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        [Required] 
        public int MovieId { get; set; }

        public Movie Movie { get; set; } = null!;
    }
}
