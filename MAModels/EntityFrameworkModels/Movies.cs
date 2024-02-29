using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MAModels.EntityFrameworkModels.Identity;

namespace MAModels.EntityFrameworkModels
{
    [Table("Movies")]
    public class Movies
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

        public List<Users> UsersList {  get; set; } = new List<Users>();

        public List<Images> ImagesList { get; set; } = new List<Images>();

        public List<Tags> TagsList { get; set; } = new List<Tags>();

        public Movies() { }
    }
}
