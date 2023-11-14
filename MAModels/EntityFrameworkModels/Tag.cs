using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAModels.EntityFrameworkModels
{
    [Table("Tags")]
    public class Tag
    {
        [Key]
        [Required]
        public int TagId { get; set; }

        [Required]
        public string TagName { get; set; } = null!;

        public List<Movie> MoviesList { get; set; } = new List<Movie>();

        public Tag() { }
    }
}
