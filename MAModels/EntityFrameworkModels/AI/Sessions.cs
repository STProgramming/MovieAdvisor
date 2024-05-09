using MAModels.EntityFrameworkModels.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAModels.EntityFrameworkModels.AI
{
    [Table("Sessions")]
    public class Sessions
    {
        [Key]
        public int SessionId { get; set; }

        public List<Requests> RequestList { get; set; } = new List<Requests>();

        [Required]
        public string UserId { get; set; } = string.Empty;

        public Users User { get; set; } = null!;

        public DateTime DateTimeCreation { get; set; }
    }
}
