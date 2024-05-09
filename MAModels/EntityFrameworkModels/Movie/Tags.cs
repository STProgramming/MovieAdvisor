using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAModels.EntityFrameworkModels.Movie
{
    [Table("Tags")]
    public class Tags
    {
        [Key]
        [Required]
        public int TagId { get; set; }

        [Required]
        public string TagName { get; set; } = null!;

        public virtual List<Movies> MoviesList { get; set; } = new List<Movies>();

        public Tags() { }
    }
}
