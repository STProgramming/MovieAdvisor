using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAModels.EntityFrameworkModels
{
    [Table("Images")]
    public class Images
    {
        [Key]
        [Required]
        public int ImageId { get; set; }

        [Required]
        public string ImageName { get; set; } = null!;

        [Required]
        public string ImageExtension { get; set; } = null!;

        [Required]
        public byte[] ImageData { get; set; }

        public int MovieId { get; set; }

        public Movies Movie { get; set; } = null!;

        public Images() { }
    }
}
