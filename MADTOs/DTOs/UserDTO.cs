using MAModels.EntityFrameworkModels;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MAModels.DTO
{
    public class UserDTO
    {
        public string Name { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string EmailAddress { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public UserDTO (User user)
        {
            this.Name = user.Name;
            this.LastName = user.LastName;
            this.UserName = user.UserName;
            this.BirthDate = user.BirthDate;
            this.EmailAddress = user.EmailAddress;
        }
    }
}
