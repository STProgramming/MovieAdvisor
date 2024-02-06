using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAModels.EntityFrameworkModels
{
    [Table("Image")]
    public class Image
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

        public Movie Movie { get; set; } = null!;

        public Image() { }
    }
}
