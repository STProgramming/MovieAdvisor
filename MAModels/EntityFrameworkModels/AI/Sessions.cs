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
        public string UserId { get; set; }

        public Users User { get; set; }

        public DateTime DateTimeCreation { get; set; }
    }
}
