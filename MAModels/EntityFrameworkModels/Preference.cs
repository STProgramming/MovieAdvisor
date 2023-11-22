using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAModels.EntityFrameworkModels
{
    [Table("Preferencies")]
    public class Preference
    {
        [Key]
        [Required]
        public int ModelTrainId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string DescriptionVote { get; set; } = null!;

        [Required]
        public float Vote { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public string MovieTitle { get; set; } = null!;

        [Required]
        public string MovieDescription { get; set; } = null!;

        [Required]
        public short MovieYear { get; set; }

        [Required]
        public string MovieMaker { get; set; } = null!;

        [Required]
        public string MovieGenres { get; set; } = null!;

        public DateTime DateTimeCreation { get; set; }

        public Preference() { }
    }
}
