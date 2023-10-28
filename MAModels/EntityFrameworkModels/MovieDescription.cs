using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAModels.EntityFrameworkModels
{
    [Table("MovieDescriptions")]
    public class MovieDescription
    {
        [Key]
        [Required]
        public int MovieDescriptionId { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public int MovieTagId { get; set; }

        public Movie Movie { get; set; }

        public MovieTag MovieTag { get; set; }

    }
}
