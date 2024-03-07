using System.ComponentModel.DataAnnotations;

namespace MADTOs.DTOs.EntityFrameworkDTOs.Identity
{
    public class UsersDTO
    {
        public string Name { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        [EmailAddress]
        public string EmailAddress { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public string Nationality { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

    }
}
