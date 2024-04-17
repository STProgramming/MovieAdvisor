using System.ComponentModel.DataAnnotations;

namespace MADTOs.DTOs.ModelsDTOs.Identity
{
    public class LoginDTO
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool StayConnected { get; set; }
    }
}
