using MAModels.Enumerables;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAModels.EntityFrameworkModels
{
    [Table("MovieTags")]
    public class MovieTag
    {
        [Key]
        [Required]
        public int MovieTagsId { get; set; }

        [Required]
        public string MovieTags { get; set; }
        
        internal List<MovieDescription>? MovieTagsDescriptionsList { get; set; }

        public MovieTag() { }
    }
}
