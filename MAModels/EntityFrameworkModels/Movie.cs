using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MAModels.EntityFrameworkModels
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        [Required]
        public int MovieId { get; set; }

        [Required, NotNull]
        public string MovieTitle { get; set; }

        [Required, NotNull]
        public short MovieYearProduction { get; set; }

        [Required, NotNull]
        public string MovieDescription { get; set; }

        [Required, NotNull]
        public string MovieMaker { get; set; }

        [Required, NotNull]
        public bool IsForAdult { get; set; }

        internal byte[]? MovieImage { get; set; }

        internal List<MovieDescription>? MovieTagsList { get; set; }

        internal List<Review>? ReviewsList {  get; set; }

        public Movie() { }        
    }
}
