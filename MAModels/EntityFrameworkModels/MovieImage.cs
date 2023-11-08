using System.ComponentModel.DataAnnotations;

namespace MAModels.EntityFrameworkModels
{
    public class MovieImage
    {
        [Key]
        public int MovieImageId { get; set; }

        [Required]
        public string MovieImageName { get; set; }

        [Required]
        public string MovieImagePath { get; set; }

        [Required]
        public string MovieImageExtension { get; set; }

        public int MovieId {  get; set; }

        public Movie Movie { get; set; }

        public MovieImage() { }
    }
}
