using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MAModels.EntityFrameworkModels
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Required]
        public int UserId { get; set; }

        [Required, NotNull]
        public string Name { get; set; } = null!;

        [Required, NotNull]
        public string LastName { get; set; } = null!;

        [Required, NotNull]
        public string UserName { get; set; } = null!;

        [Required, NotNull]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;

        [Required, NotNull]
        public DateTime BirthDate { get; set; }

        public ICollection<Movie>? MoviesList { get; set; } = new List<Movie>();

        public ICollection<Review> ReviewsList { get; set; } = new List<Review>();
    }
}
