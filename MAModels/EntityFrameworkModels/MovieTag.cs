using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAModels.EntityFrameworkModels
{
    [Table("MoviesTags")]
    public class MovieTag
    {
        [Key]
        [Required]
        public int MovieTagId { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public int TagId { get; set; }

        public Movie Movie { get; set; } = null!;

        public Tag Tag { get; set; } = null!;

        public MovieTag() { }
    }
}
