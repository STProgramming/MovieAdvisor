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

        public virtual Movie Movie { get; set; } = null!;

        public virtual User User { get; set; } = null!;      

        public MovieUser() { }
    }
}
