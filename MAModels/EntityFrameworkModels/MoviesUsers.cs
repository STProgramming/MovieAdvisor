using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAModels.EntityFrameworkModels
{
    [Table("MoviesUsers")]
    public class MovieUser
    {
        [Key]
        [Required]
        public int MovieUserId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int MovieId { get; set; }

        public Movie Movie { get; set; } = null!;

        public User User { get; set; } = null!;      
    }
}
