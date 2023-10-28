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
        public string Name { get; set; }

        [Required, NotNull]
        public string LastName { get; set; }

        [Required, NotNull]
        public string UserName { get; set; }

        [Required, NotNull]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required, NotNull]
        public DateTime BirthDate { get; set; }

        public List<Review>? Reviews { get; set; }

        public User() { }
    }
}
