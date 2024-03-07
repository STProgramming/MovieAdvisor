using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MADTOs.DTOs.ModelsDTOs
{
    public class NewUserDTO
    {
        [Required, NotNull]
        public string Name { get; set; } = string.Empty;

        [Required, NotNull]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress, NotNull]
        public string Email { get; set; } = string.Empty;

        [Required, NotNull]
        public string UserName { get; set; } = string.Empty;

        [Required, NotNull]
        public DateTime BirthDate { get; set; }

        [Required, NotNull]
        public string Password { get; set; } = string.Empty;
    }
}
