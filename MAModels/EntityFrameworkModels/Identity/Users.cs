using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MAModels.EntityFrameworkModels.Identity
{
    [Table("Users")]
    public class Users : IdentityUser
    {
        [Required, NotNull]
        public string Name { get; set; } = string.Empty;

        [Required, NotNull]
        public string LastName { get; set; } = string.Empty;

        [Required, NotNull]
        public DateTime BirthDate { get; set; }

        public List<Movies> MoviesList { get; set; } = new List<Movies>();

        public List<Reviews> ReviewsList { get; set; } = new List<Reviews>();

        public Users() { }
    }
}
