using MAModels.EntityFrameworkModels.AI;
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

        [Required, NotNull]
        public string Nationality { get; set; } = string.Empty;

        [Required, NotNull]
        public string Gender {  get; set; } = string.Empty;

        public List<Movies> MoviesList { get; set; } = new List<Movies>();

        public List<Reviews> ReviewsList { get; set; } = new List<Reviews>();

        public List<Sessions> SessionsList { get; set; } = new List<Sessions>();

        public Users() { }
    }
}
