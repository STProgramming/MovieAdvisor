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
        public string MovieTitle { get; set; } = null!;

        [Required, NotNull]
        public short MovieYearProduction { get; set; }

        [Required, NotNull]
        public string MovieDescription { get; set; } = null!;

        [Required, NotNull]
        public string MovieMaker { get; set; } = null!;

        [Required, NotNull]
        public bool IsForAdult { get; set; }

        public ICollection<User>? UsersList {  get; set; } = new List<User>();

        public ICollection<Image> ImagesList { get; set; } = new List<Image>();

        internal ICollection<Tag> TagsList { get; set; } = new List<Tag>();
    }
}
