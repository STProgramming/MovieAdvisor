using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAModels.DTO
{
    public class UserDTO
    {
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
    }
}
